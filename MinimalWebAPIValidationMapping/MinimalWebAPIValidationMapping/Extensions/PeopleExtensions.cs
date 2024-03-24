using MinimalWebAPIValidationMapping.Dtos;
using MinimalWebAPIValidationMapping.Entities;

namespace MinimalWebAPIValidationMapping.Extensions;

public static class PeopleExtensions
{
    public static PersonDto ToDto(this PersonEntity personEntity)
    => new()
    {
        Id = personEntity.Id,
        FirstName = personEntity.FirstName,
        LastName = personEntity.LastName,
        Age = CalculateAge(personEntity.BirthDate),
        City = personEntity.Address?.City
    };

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
