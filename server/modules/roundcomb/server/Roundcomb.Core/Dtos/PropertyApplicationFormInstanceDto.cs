using System;
using ProductManagement.Core.Dto;
using Membership.Core.Dto;
using Roundcomb.Core.Entities;

namespace Roundcomb.Core.Dtos
{
	public class PropertyApplicationFormInstanceDto : PropertyApplicationFormInstanceBase
	{
		public PropertyDto Property { get; set; }

		public UserPersonalInformation UserInformation { get; set; }

		public string[] FileNames => UploadedApplicationFileName?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries) ?? new string[] { };

		public string[] FileUrls => UploadedApplicationFileUrl?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries) ?? new string[] { };
	}
}