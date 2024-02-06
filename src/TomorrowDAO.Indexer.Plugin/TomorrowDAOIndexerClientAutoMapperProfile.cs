using AElfIndexer.Client.Handlers;
using AutoMapper;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using TomorrowDAO.Contracts.DAO;
using GovernanceSchemeThresholdIndex = TomorrowDAO.Indexer.Plugin.Entities.GovernanceSchemeThreshold;
using GovernanceSchemeThresholdContract = TomorrowDAO.Contracts.DAO.GovernanceSchemeThreshold;
using HighCouncilConfigContract = TomorrowDAO.Contracts.DAO.HighCouncilConfig;
using HighCouncilConfigIndexer = TomorrowDAO.Indexer.Plugin.Entities.HighCouncilConfig;
using MetadataContract = TomorrowDAO.Contracts.DAO.Metadata;
using MetadataIndexer = TomorrowDAO.Indexer.Plugin.Entities.Metadata;

namespace TomorrowDAO.Indexer.Plugin;

public class TomorrowDAOIndexerClientAutoMapperProfile : Profile
{
    public TomorrowDAOIndexerClientAutoMapperProfile()
    {
        CreateMap<LogEventContext, ProposalIndex>();
        CreateMap<ProposalCreated, ProposalIndex>()
            .ForMember(des => des.DAOId, opt
                => opt.MapFrom(source => source.DaoId.ToHex()
                ))
            .ForMember(des => des.ProposalId, opt
                => opt.MapFrom(source => source.ProposalId.ToHex()
                ))
            .ForMember(des => des.GovernanceSchemeId, opt
                => opt.MapFrom(source => source.GovernanceSchemeId.ToHex()
                ))
            .ForMember(des => des.VoteSchemeId, opt
                => opt.MapFrom(source => source.VoteSchemeId.ToHex()
                ))
            .ForMember(des => des.OrganizationAddress, opt
                => opt.MapFrom(source => source.OrganizationAddress != null ? source.OrganizationAddress.ToBase58() : null
                ))
            .ForMember(des => des.ReleaseAddress, opt
                => opt.MapFrom(source => source.ReleaseAddress != null ? source.ReleaseAddress.ToBase58() : null
                ));
        CreateMap<ProposalReleased, ProposalIndex>();
        CreateMap<GovernanceSubSchemeIndex, ProposalIndex>(); 
        CreateMap<ProposalIndex, ProposalSyncDto>();
        CreateMap<LogEventContext, DAOIndex>();
        CreateMap<DAOCreated, DAOIndex>();
        CreateMap<GovernanceSchemeThresholdContract, GovernanceSchemeThresholdIndex>();
        CreateMap<HighCouncilConfigContract, HighCouncilConfigIndexer>();
        CreateMap<MetadataContract, MetadataIndexer>();
    }
}