using Core.ObjectMapping;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using System.Linq;

namespace ProductManagement.Core.MappingConfigrations
{
	public class PropertyManagementMappingRegistration : IObjectMappingRegistration
	{
		public void ConfigureMapping(IMappingService map)
		{
			map.ConfigureMapping<Property, PropertyDto>()
			   .ForMember(dto => dto.Tags, a => a.MapFrom(entity => entity.Tags.Select(tag => tag.Name)))
			   .ForMember(dto => dto.CategoryId,
						  a => a.MapFrom(entity => entity.Category == null ? 0 : entity.Category.Id))
			   .ForMember(dto => dto.Images, a => a.MapFrom(entity => entity.Images.Where(img => !img.IsDeleted)))
			   .ForMember(dto => dto.Variants,
						  a => a.MapFrom(entity => entity.PropertyVariants == null ? 0 : entity.PropertyVariants.Count(var => !var.IsDeleted)))
			   .ForMember(dto => dto.ExtendedProperties, a => a.Ignore())

			   .MaxDepth(2)
				;
			map.ConfigureMapping<Property, PropertyWithSettingDto>()
			   .ForMember(dto => dto.Tags, a => a.MapFrom(entity => entity.Tags.Select(tag => tag.Name)))
			   .ForMember(dto => dto.CategoryId, a => a.MapFrom(entity => entity.Category == null ? 0 : entity.Category.Id))
			   .ForMember(dto => dto.Images, a => a.Ignore())// a => a.MapFrom(entity => entity.Images.Where(img => !img.IsDeleted)))
			   .ForMember(dto => dto.ExtendedProperties, a => a.Ignore())

			   .MaxDepth(2)
				;

			map.ConfigureMapping<Vendor, VendorDto>().ReverseMap();

			map.ConfigureMapping<Property, FeaturedPropertyDto>()
			   .ForMember(dto => dto.Tags, a => a.MapFrom(entity => entity.Tags.Select(tag => tag.Name)))
			   .ForMember(dto => dto.CategoryId, a => a.MapFrom(entity => entity.Category == null ? (long?)null : entity.Category.Id))
			   .ForMember(dto => dto.Images, a => a.MapFrom(entity => entity.Images.Where(img => !img.IsDeleted).ToList()))
			   .ForMember(dto => dto.ExtendedProperties, a => a.Ignore())

			   .MaxDepth(3)
				;

			map.ConfigureMapping<PropertyDto, Property>()
			   .ForMember(entity => entity.Id, a => a.Ignore())
			   .ForMember(entity => entity.Category, a => a.Ignore())
			   .ForMember(entity => entity.Tags, a => a.Ignore())
               .ForMember(entity => entity.PropertyVariants, a => a.Ignore())
               .ForMember(entity => entity.Images, a => a.Ignore())
			   .ForMember(entity => entity.OwnerId, a => a.Ignore())
				;

			map.ConfigureMapping<PropertyWithSettingDto, Property>()
			   .ForMember(entity => entity.Id, a => a.Ignore())
			   .ForMember(entity => entity.Category, a => a.Ignore())
			   .ForMember(entity => entity.Tags, a => a.Ignore())
			   .ForMember(entity => entity.Images, a => a.Ignore())
               .ForMember(entity => entity.PropertyVariants, a => a.Ignore())
               .ForMember(entity => entity.OwnerId, a => a.Ignore())
				;

			map.ConfigureMapping<PropertyCart, PropertyCartDto>();

			map.ConfigureMapping<PropertyCartDto, PropertyCart>()
			   .ForMember(entity => entity.Items, a => a.Ignore());

			map.ConfigureMapping<PropertyCartItem, PropertyCartItemDto>().ReverseMap();

			map.ConfigureMapping<PropertyDto, FeaturedPropertyDto>();

			map.ConfigureMapping<PropertyCategory, PropertyCategoryDto>()
			   .ForMember(dto => dto.Children, a => a.MapFrom(entity => entity.Children.Where(category => !category.IsDeleted)))
			   .MaxDepth(5);

			map.ConfigureMapping<FeaturedCategory, FeaturedCategoryDto>();
			map.ConfigureMapping<FeaturedCategoryDto, FeaturedCategory>()
				.ForMember(entity => entity.Category, a => a.Ignore());

			map.ConfigureMapping<PropertyCategoryDto, PropertyCategory>()
			   .ForMember(entity => entity.Children, a => a.Ignore())
				;

			map.ConfigureMapping<PropertyImage, PropertyImageDto>().ReverseMap();

			map.ConfigureMapping<Tag, TagDto>()
			   .ForMember(dto => dto.ChildrenTags,
			              a => a.MapFrom(entity => entity.ChildrenTags.Where(x => !x.IsDeleted)))
			   .ForMember(dto => dto.PropertyCounts,
			              a => a.MapFrom(entity => entity.Properties == null || !entity.Properties.Any()
				                                       ? 0
				                                       : entity.Properties.Count(
					                                       x => !x.IsDeleted && x.IsActive &&
					                                            x.StatusString ==
					                                            PropertyStatus.AvailableForSell.ToString())))
				;


			map.ConfigureMapping<TagDto, Tag>()
			   .ForMember(entity => entity.ChildrenTags, a => a.Ignore());

			map.ConfigureMapping<PropertyContactMessage, PropertyContactMessageDto>()
			   .ForMember(dto => dto.ThreadGuid, a => a.MapFrom(entity => entity.ThreadGuid))
				;

			map.ConfigureMapping<PropertyContactMessageDto, PropertyContactMessage>()
			   .ForMember(entity => entity.Property, a => a.Ignore())
			   .ForMember(entity => entity.ThreadGuid, a => a.Ignore())
			   .ForMember(entity => entity.ReplyToMessage, a => a.Ignore())
				;

			map.ConfigureMapping<PropertyHistory, PropertyHistoryDto>();
			map.ConfigureMapping<PropertyHistoryDto, PropertyHistory>()
			   .ForMember(entity => entity.Property, a => a.Ignore())
				;

			map.ConfigureMapping<PropertyImportMappingSet, PropertyImportMappingSetDto>();
			map.ConfigureMapping<PropertyImportMappingSetDto, PropertyImportMappingSet>();

			map.ConfigureMapping<PropertySearchRecord, PropertySearchRecordDto>();
			map.ConfigureMapping<PropertySearchRecordDto, PropertySearchRecord>();
		}
	}
}