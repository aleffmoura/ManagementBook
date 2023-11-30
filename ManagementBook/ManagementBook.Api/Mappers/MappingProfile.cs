namespace ManagementBook.Api.Mappers;

using AutoMapper;
using ManagementBook.Api.DTOs;
using ManagementBook.Application.Features.Books.Commands;
using ManagementBook.Domain.Books;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookCreateDto, BookSaveCommand>()
            .ForMember(ds => ds.Id, m => m.MapFrom(src => Guid.NewGuid()))
            .ForMember(ds => ds.Released, m => m.MapFrom(src => src.ReleaseData));
        CreateMap<BookSaveCommand, Book>()
            .ForMember(ds => ds.Id, m => m.MapFrom(src => src.Id))
            .ForMember(ds => ds.ReleaseDate, m => m.MapFrom(src => src.Released));
    }
}
