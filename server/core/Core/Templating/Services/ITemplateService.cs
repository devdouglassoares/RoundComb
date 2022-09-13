using Core.Exceptions;
using Core.Templating.Data.Entities;
using Core.Templating.Data.Repositories;
using Core.Templating.Models;
using Core.Templating.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Templating.Services
{
	public interface ITemplateService : IDependency
	{
		Dictionary<string, string> GetAllAvailableTemplates();

		void SaveTemplate(string templateType, TemplateModelDto content);

		TemplateModelDto LoadTemplate<TTemplate>() where TTemplate : ITemplateModel;

		TemplateModelDto LoadTemplate(string templateModelType);

		TemplateModelDto ParseTemplate<TTemplate>(TTemplate instance) where TTemplate : ITemplateModel;

		string ParseTemplate(string template, dynamic instance);
	}

	public class TemplateService : ITemplateService
	{
		private readonly ITemplatingRepository _repository;

		private readonly IEnumerable<ITemplateModel> _templateModels;

		public TemplateService(ITemplatingRepository repository, IEnumerable<ITemplateModel> templateModels)
		{
			_repository = repository;
			_templateModels = templateModels;
		}

		public Dictionary<string, string> GetAllAvailableTemplates()
		{
			var allAvailableTemplates = _repository.Fetch<TemplateModel>(x => !x.IsDeleted)
												   .ToArray();

			return allAvailableTemplates
				.ToDictionary(x =>
							  {
								  var name =
									  x.TemplateTypeString.Split(new[] { '.' },
										  StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
								  return name;
							  }, y => y.TemplateTypeString);
		}

		public void SaveTemplate(string templateType, TemplateModelDto content)
		{
			var typeName = templateType;
			var templateObject = _repository.First<TemplateModel>(template => template.TemplateTypeString == typeName);

			if (templateObject == null || templateObject.IsDeleted)
				throw new BaseNotFoundException<TemplateModel>();

			templateObject.TemplateTitle = content.TemplateTitle;
			templateObject.TemplateContent = content.TemplateContent;
			templateObject.ModifiedDate = DateTimeOffset.Now;
			_repository.Update(templateObject);
			_repository.SaveChanges();
		}

		public TemplateModelDto LoadTemplate<TTemplate>() where TTemplate : ITemplateModel
		{
			return LoadTemplate(typeof(TTemplate).FullName);
		}

		public TemplateModelDto LoadTemplate(string templateModelType)
		{
			var typeName = templateModelType;

			var templateModel = _repository.First<TemplateModel>(template => template.TemplateTypeString == typeName);

			return new TemplateModelDto
			{
				TemplateContent = templateModel.TemplateContent,
				TemplateTypeString = templateModel.TemplateTypeString,
				TemplateTitle = templateModel.TemplateTitle,
				Fields = templateModel.Fields
			};
		}

		public TemplateModelDto ParseTemplate<TTemplate>(TTemplate instance) where TTemplate : ITemplateModel
		{
			var template = LoadTemplate(typeof(TTemplate).FullName);

			if (string.IsNullOrEmpty(template?.TemplateContent) && string.IsNullOrEmpty(template?.TemplateTitle))
				return null;

			var baseTemplateModel = instance as BaseTemplateModel;
			if (baseTemplateModel != null)
			{
				if (HttpContext.Current != null && string.IsNullOrEmpty(baseTemplateModel.SiteUrl))
					baseTemplateModel.SiteUrl = new Uri(HttpContext.Current.Request.Url, "/").ToString().Trim('/');
			}

			template.TemplateContent = TemplateParser.Parse(template.TemplateContent, instance);
			template.TemplateTitle = TemplateParser.Parse(template.TemplateTitle, instance);

			return template;
		}

		public string ParseTemplate(string template, dynamic instance)
		{
			try
			{

				if (HttpContext.Current != null && string.IsNullOrEmpty(instance.SiteUrl))
					instance.SiteUrl = new Uri(HttpContext.Current.Request.Url, "/").ToString().Trim('/');
			}
			finally
			{
				// trying to set site url failed, just ignore the error.
			}

			return TemplateParser.Parse(template, instance);
		}
	}
}