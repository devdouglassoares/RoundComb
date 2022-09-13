using System.Collections;
using Core;
using Core.Database;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagement.Core.Services
{
	public interface IPropertyService : IBaseService<Property, PropertyDto>, IDependency
	{
		IEnumerable<FeaturedPropertyDto> GetFeaturedProperties();

		PropertyDto CreateAndGetPropertyVariant(long propertyId);

        IEnumerable<PropertyDto> GetSimilarsForProperty(long propertyId);

        IEnumerable<PropertyDto> GetVariantsForProperty(long propertyId);

		PropertyWithSettingDto GeneratePropertyDto();

		List<Hashtable> GetPropertyDataForPrint(PropertyQueryRequestModel filterModel);

		void MovePropertiesToCategory(long[] propertyIds, long categoryId);

		PropertyQueryResult QueryProperties(PropertyQueryRequestModel model, bool pagination = true);

        PropertyQueryResult SearchProperties(PropertyQueryRequestModel model, bool pagination = true);


        PropertyCartDto AddToCart(long propertyId, int quantity);

		void UpdatePropertySellType(long[] propertyIds, string selectedSellType);

		void UpdatePropertyStatus(long[] propertyIds, string selectedStatus);

		void SendPropertySuggestion(long userId, long[] propertyIds);

		IEnumerable<PropertyDto> GetDtos(long[] propertyIds);

		void MarkPropertiesAsInactiveBeforeSynchronizing(string[] activePropertiesExternalCodes);

		void ImportProperty(string propertyCode, PropertyDto model, bool fireImportEvent = true);

		string ExportProperties();

		void AssignPropertiesToTags(long[] propertyIds, string[] tags);

	    IQueryable<Property> FetchDeleted();

	    void UndoDeletionOfProperty(long propertyId);

	}
}