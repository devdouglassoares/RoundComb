let gulp = require('gulp');
let del = require('del');
let shell = require('gulp-shell');
let _ = require('lodash-node');
let modulesConfig = require('./modules.json');


gulp.task('nuget-restore', shell.task(['nuget.exe restore']));

gulp.task('build', function () {
	let moduleParams = _.find(process.argv, function (params) {
		return _.startsWith(params, '--modules');
	});
	let debugMode = false;

	if (moduleParams === null)
		throw new Error('modules parameter not found');

	let debugParam = _.find(process.argv, function (params) {
		return _.startsWith(params, '--debug');
	});

	if (debugParam)
		debugMode = true;

	let customDestFolder = '%PORTAL_DIR%';
	let destFolder = _.find(process.argv, function (params) {
		return _.startsWith(params, '--destFolder');
	});

	if (destFolder)
		customDestFolder = destFolder.replace('--destFolder=', '');

	let destinationFolder = customDestFolder + '\\' + (debugMode ? "serverside" : "Release") + '\\';

	console.log('destinationFolder', destinationFolder);

	let modulesToBuild = moduleParams.replace('--modules=', '').split(',');

	let modulesStack = [];

	function getModulesStack(modules) {
		modules.forEach(function (module) {
			modulesConfig.forEach(function (m) {
				if (m.name == module) {
					getModulesStack(m.dependencies);
					modulesStack = _.uniq(modulesStack.concat(m.dependencies));
				}
			});
			modulesStack.push(module);
		});
	}

	getModulesStack(modulesToBuild);

	let moduleBuildConfig = [];
	modulesStack.forEach(function (name) {
		let module = _.find(modulesConfig, function (config) {
			return config.name == name;
		});
		if (!module) {
			throw new Error("Dependency module cannot be found");
		}
		moduleBuildConfig.push(module);
	});

	let dependencies = [];

	moduleBuildConfig.forEach(function (module) {
		if (dependencies.indexOf('Build_______Module_' + module.name) > -1)
			return;

		let commandsToRun = [];
		let options = {
			cwd: process.cwd() + '\\' + module.path.replace(/\//g, '\\'),
			maxBuffer: 256 * 1024 * 1024
		};

		commandsToRun.push('@echo Building module: ' + module.name + ' from folder ' + options.cwd);

		commandsToRun.push('nuget.exe restore');
		let command = 'msbuild.exe ' + module.solutionFile + ' ' +
			'/target:Clean;Build ' +
			'/verbosity:q ' +
			'/nologo ' +
			'/p:Platform="Any CPU" ' +
			'/maxcpucount ';

		if (debugMode)
			command += '/p:Configuration=Debug ';
		else
			command += '/p:Configuration=Release ';
		//
		// // only publish modules that need publishing
		// if (modulesToBuild.indexOf(module.name) > -1 && module.publishBaseFolder) {
		//   if (!debugMode) {
		//     command += '/property:DeployOnBuild="true" ' +
		//       '/property:PublishProfile="Publish.pubxml" ' +
		//       '/property:PublishProfileRootFolder="%cd%\\' + module.publishBaseFolder + '\\Properties\\PublishProfiles"';
		//   }
		// }

		commandsToRun.push(command);

		if (modulesToBuild.indexOf(module.name) > -1 && module.publishWebProject) {
			let debugBuildCommand = 'msbuild.exe ' + module.publishWebProject + ' ' +
				'/target:Clean;Build ' +
				'/verbosity:q ' +
				'/nologo ' +
				'/p:Platform=AnyCPU ' +
				'/maxcpucount ' +
				'/p:Configuration="' + (debugMode ? "Debug" : "Release") + '" ';

			debugBuildCommand += '/t:WebPublish ' +
				'/p:WebPublishMethod=FileSystem ' +
				'/p:DeleteExistingFiles=True ' +
				'/p:publishUrl="' + destinationFolder + module.publishFolderName + '" ';

			commandsToRun.push(debugBuildCommand);
		}

		if (!!module.deployWinService) {
			let folder = 'Debug';
			if (!debugMode) {
				folder = 'Release';
			}

			['exe', 'dll', 'config', 'application', 'manifest'].forEach(function (ext) {
				commandsToRun.push('xcopy /y /e /q %cd%\\' + module.deployWinService + '\\bin\\' + folder + '\\*.' + ext + ' ' + destinationFolder + module.deployWinService + '\\');
			});
		}

		if (dependencies.length)
			gulp.task('Build_______Module_' + module.name, [dependencies[dependencies.length - 1]], shell.task(commandsToRun, options));
		else
			gulp.task('Build_______Module_' + module.name, shell.task(commandsToRun, options));

		dependencies.push('Build_______Module_' + module.name);
	});

	gulp.task('BuildAllModules', function () {
		gulp.start(dependencies[dependencies.length - 1]);
	});

	gulp.start('BuildAllModules');
});
