using AElfIndexer.Client.Handlers;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using GovernanceSchemeThresholdIndex = TomorrowDAO.Indexer.Plugin.Entities.GovernanceSchemeThreshold;
using GovernanceSchemeThresholdContract = TomorrowDAO.Contracts.DAO.GovernanceSchemeThreshold;
using HighCouncilConfigContract = TomorrowDAO.Contracts.DAO.HighCouncilConfig;
using HighCouncilConfigIndexer = TomorrowDAO.Indexer.Plugin.Entities.HighCouncilConfig;

namespace TomorrowDAO.Indexer.Plugin;

public class TomorrowDAOIndexerClientAutoMapperProfile : IndexerMapperBase
{
    public TomorrowDAOIndexerClientAutoMapperProfile()
    {
        CreateMap<LogEventContext, ProposalIndex>();
        CreateMap<ProposalCreated, ProposalIndex>()
            .ForMember(des => des.DAOId, opt
                => opt.MapFrom(source => MapHash(source.DaoId)))
            .ForMember(des => des.ProposalId, opt
                => opt.MapFrom(source => MapHash(source.ProposalId)))
            .ForMember(des => des.GovernanceSchemeId, opt
                => opt.MapFrom(source => MapHash(source.GovernanceSchemeId)))
            .ForMember(des => des.VoteSchemeId, opt
                => opt.MapFrom(source => MapHash(source.VoteSchemeId)))
            .ForMember(des => des.OrganizationAddress, opt
                => opt.MapFrom(source => MapAddress(source.OrganizationAddress)))
            .ForMember(des => des.ReleaseAddress, opt
                => opt.MapFrom(source => MapAddress(source.ReleaseAddress)))
            .ForMember(des => des.StartTime, opt
                => opt.MapFrom(source => MapDateTime(source.StartTime)))
            .ForMember(des => des.EndTime, opt
                => opt.MapFrom(source => MapDateTime(source.EndTime)))
            .ForMember(des => des.ExpiredTime, opt
                => opt.MapFrom(source => MapDateTime(source.ExpiredTime)));
        
        CreateMap<ProposalReleased, ProposalIndex>();
        CreateMap<GovernanceSubSchemeIndex, ProposalIndex>();
        CreateMap<ProposalIndex, ProposalSyncDto>();
        CreateMap<LogEventContext, DAOIndex>();
        CreateMap<DAOCreated, DAOIndex>()
            .ForMember(des => des.DAOMetadata, opt => opt.MapFrom(source => source.Metadata));
        CreateMap<GovernanceSchemeThresholdContract, GovernanceSchemeThresholdIndex>();
        CreateMap<HighCouncilConfigContract, HighCouncilConfigIndexer>();
        CreateMap<Metadata, DAOMetadata>();
        CreateMap<LogEventContext, OrganizationIndex>();
        CreateMap<OrganizationCreated, OrganizationIndex>()
            .ForMember(des => des.OrganizationAddress, opt
                => opt.MapFrom(source => MapAddress(source.OrganizationAddress)))
            .ForMember(des => des.GovernanceSchemeId, opt
                => opt.MapFrom(source => MapHash(source.GovernanceSchemeId)))
            .ForMember(des => des.OrganizationMemberSet, opt 
                => opt.MapFrom(source => MapOrganizationMemberSet(source)));
        CreateMap<LogEventContext, GovernanceSchemeIndex>();
        CreateMap<LogEventContext, GovernanceSubSchemeIndex>();
        CreateMap<GovernanceSchemeCreated, GovernanceSchemeIndex>()
            .ForMember(des => des.GovernanceSchemeId, opt
                => opt.MapFrom(source => MapHash(source.GovernanceSchemeId)))
            .ForMember(des => des.Creator, opt
                => opt.MapFrom(source => MapAddress(source.Creator)));
        CreateMap<GovernanceSubSchemeAdded, GovernanceSubSchemeIndex>()
            .ForMember(des => des.SubSchemeId, opt
                => opt.MapFrom(source => MapHash(source.SubSchemeId)))
            .ForMember(des => des.ParentSchemeId, opt
                => opt.MapFrom(source => MapHash(source.ParentSchemeId)));
    }
}