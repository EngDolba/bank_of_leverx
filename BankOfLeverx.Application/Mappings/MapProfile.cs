using AutoMapper;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Loan, LoanDTO>();
        CreateMap<LoanDTO, Loan>();
        CreateMap<LoanPatchDTO, Loan>()
           .ForMember(dest => dest.Amount, opt =>
               opt.Condition(src => src.Amount.HasValue))
           .ForMember(dest => dest.InitialAmount, opt =>
               opt.Condition(src => src.InitialAmount.HasValue))
           .ForMember(dest => dest.StartDate, opt =>
               opt.Condition(src => src.StartDate.HasValue))
           .ForMember(dest => dest.EndDate, opt =>
               opt.Condition(src => src.EndDate.HasValue))
           .ForMember(dest => dest.Rate, opt =>
               opt.Condition(src => src.Rate.HasValue))
           .ForMember(dest => dest.Type, opt =>
               opt.Condition(src => src.Type != null))
           .ForMember(dest => dest.BankerKey, opt =>
               opt.Condition(src => src.BankerKey.HasValue))
           .ForMember(dest => dest.AccountKey, opt =>
               opt.Condition(src => src.AccountKey.HasValue));

    }
}