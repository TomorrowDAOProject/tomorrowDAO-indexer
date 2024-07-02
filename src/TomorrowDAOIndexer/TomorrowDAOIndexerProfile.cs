using TomorrowDAO.Contracts.DAO;
using TomorrowDAOIndexer.Dto;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer;

public class TomorrowDAOIndexerProfile : IndexerMapperBase
{
   public TomorrowDAOIndexerProfile()
    {
        // DAO
        CreateMap<DAOIndex, DAOInfoDto>();
        CreateMap<DAOCreated, DAOIndex>()
            .ForMember(des => des.LogoUrl, opt
                => opt.MapFrom(source => source.Metadata.LogoUrl))
            .ForMember(des => des.Name, opt
                => opt.MapFrom(source => source.Metadata.Name))
            .ForMember(des => des.Description, opt
                => opt.MapFrom(source => source.Metadata.Description))
            .ForMember(des => des.SocialMedia, opt
                => opt.MapFrom(source => source.Metadata.SocialMedia))
            .ForMember(dest => dest.Metadata, opt => opt.Ignore())
            .ForMember(des => des.IsNetworkDAO, opt
                => opt.MapFrom(source => source.IsNetworkDao))
            .ForMember(des => des.Creator, opt
                => opt.MapFrom(source => MapAddress(source.Creator)))
            .ForMember(des => des.Id, opt
                => opt.MapFrom(source => MapHash(source.DaoId)))
            .ForMember(des => des.TreasuryContractAddress, opt
                => opt.MapFrom(source =>
                    source.ContractAddressList == null
                        ? string.Empty
                        : MapAddress(source.ContractAddressList.TreasuryContractAddress)))
            .ForMember(des => des.VoteContractAddress, opt
                => opt.MapFrom(source =>
                    source.ContractAddressList == null
                        ? string.Empty
                        : MapAddress(source.ContractAddressList.VoteContractAddress)))
            .ForMember(des => des.ElectionContractAddress, opt
                => opt.MapFrom(source =>
                    source.ContractAddressList == null
                        ? string.Empty
                        : MapAddress(source.ContractAddressList.ElectionContractAddress)))
            .ForMember(des => des.GovernanceContractAddress, opt
                => opt.MapFrom(source =>
                    source.ContractAddressList == null
                        ? string.Empty
                        : MapAddress(source.ContractAddressList.GovernanceContractAddress)))
            .ForMember(des => des.TimelockContractAddress, opt
                => opt.MapFrom(source =>
                    source.ContractAddressList == null
                        ? string.Empty
                        : MapAddress(source.ContractAddressList.TimelockContractAddress)))
            .ForMember(des => des.TreasuryAccountAddress, opt 
                => opt.MapFrom(source => MapAddress(source.TreasuryAddress)))
            ;
    }
}