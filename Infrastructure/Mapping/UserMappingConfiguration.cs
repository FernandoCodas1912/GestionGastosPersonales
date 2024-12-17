using Core.DTOs;
using Core.Entities;
using Mapster;

namespace Infrastructure.Mapping
{
    public class UserMappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserDTO, User>()
                .Map(dest => dest.FullName, src => src.FullName)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Password, src => BCrypt.Net.BCrypt.HashPassword(src.Password))
                .Map(dest => dest.CreationDate, src => DateTime.UtcNow)
                .Map(dest => dest.UpdateDate, src => (DateTime?)null)
                ;
        }

    }
}
