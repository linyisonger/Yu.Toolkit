# Yu.Toolkit
This is a small c sharp toolkit.

## Yu.Toolkit.RegionCode

- property
  - Regions
  - Provinces
  - Cities
  - Areas
- method
  - ```RegionDto GetRegionByCode(string code)```
  - ```RegionDto GetRegionByName(string name)```
  - ```string GetRegionNameByCode(string code)```
  - ```string GetRegionCodeByName(string name)```
  - ```RegionDto GetProvinceByCode(string code)```
  - ```RegionDto GetProvinceByName(string name)```
  - ```GetProvinceNameByCode(string code)```
  - ```GetProvinceCodeByName(string name)```
  - ```RegionDto GetCityByCode(string code)```
  - ```string GetCityNameByCode(string code)```
  - ```string GetCityCodeByName(string name)```
  - ```RegionDto GetAreaByCode(string code)```
  - ```RegionDto GetAreaByName(string name)```
  - ```string GetAreaNameByCode(string code)```
  - ```string GetAreaCodeByName(string name)```
  - ```List<RegionDto> GetCitiesByProvinceCode(string code)```
  - ```List<RegionDto> GetAreasByCityCode(string code)``` 

## Yu.Toolkit.UnifiedSocialCreditIdentifier

* property
  * RegistrationManagementDepartmentCodeList
* method
  * ```string GetCheckCode(string str)```
  * ``` void CheckOrganizationCode(string code)```
  * ```string Gengenerate(string[] organizationCodes = null, string regionCode = "")```
  * ```void Verification(string code)```

## Yu.Toolkit.CitizenIdentificationNumber

* method
  * ```string GetCheckCode(string str)```
  * ``` void CheckOrganizationCode(string code)```
  * ```string Gengenerate(string regionCode = "", string dateBirth = "", CitizenIdentificationNumberGenderEnum gender = CitizenIdentificationNumberGenderEnum.Male | CitizenIdentificationNumberGenderEnum.Female)```
  * ````void Verification(string code)```

