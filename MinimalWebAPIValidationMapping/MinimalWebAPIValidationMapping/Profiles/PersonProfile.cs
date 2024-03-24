using AutoMapper;

using MinimalWebAPIValidationMapping.Dtos;
using MinimalWebAPIValidationMapping.Entities;

namespace MinimalWebAPIValidationMapping.Profiles;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<PersonEntity, PersonDto>()
            .ForMember(dst => dst.Age, opt => opt.MapFrom(src => CalculateAge(src.BirthDate)))
            .ForMember(dst => dst.City, opt => opt.MapFrom(src => src.Address.City));
    }

    private static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (today.DayOfYear < dateOfBirth.DayOfYear)
        {
            age--;
        }

        return age;
    }
}
