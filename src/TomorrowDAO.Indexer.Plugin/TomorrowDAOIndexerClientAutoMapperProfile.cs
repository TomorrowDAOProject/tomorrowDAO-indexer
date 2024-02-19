using AElfIndexer.Client.Handlers;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using HighCouncilConfigContract = TomorrowDAO.Contracts.DAO.HighCouncilConfig;
using HighCouncilConfigIndexer = TomorrowDAO.Indexer.Plugin.Entities.HighCouncilConfig;
using MetadataContract = TomorrowDAO.Contracts.DAO.Metadata;
using MetadataIndexer = TomorrowDAO.Indexer.Plugin.Entities.Metadata;
using FileInfoIndexer = TomorrowDAO.Indexer.Plugin.Entities.FileInfo;
using FileInfoContract = TomorrowDAO.Contracts.DAO.FileInfo;
using FileIndexer = TomorrowDAO.Indexer.Plugin.Entities.File;
using FileContract = TomorrowDAO.Contracts.DAO.File;

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
            .ForMember(des => des.ExecuteAddress, opt
                => opt.MapFrom(source => MapAddress(source.ExecuteAddress)))
            .ForMember(des => des.StartTime, opt
                => opt.MapFrom(source => MapDateTime(source.StartTime)))
            .ForMember(des => des.EndTime, opt
                => opt.MapFrom(source => MapDateTime(source.EndTime)))
            .ForMember(des => des.ExpiredTime, opt
                => opt.MapFrom(source => MapDateTime(source.ExpiredTime)));
        
        CreateMap<ProposalExecuted, ProposalIndex>();
        CreateMap<GovernanceSubSchemeIndex, ProposalIndex>();
        CreateMap<ProposalIndex, ProposalSyncDto>();
        CreateMap<LogEventContext, DAOIndex>();
        CreateMap<DAOCreated, DAOIndex>()
            .ForMember(des => des.Creator, opt
                => opt.MapFrom(source => MapAddress(source.Creator)))
            .ForMember(des => des.Id, opt
                => opt.MapFrom(source => MapHash(source.DaoId)))
            .ForMember(des => des.TreasuryContractAddress, opt
                => opt.MapFrom(source => source.ContractAddressList == null ? 
                    string.Empty : MapAddress(source.ContractAddressList.TreasuryContractAddress)))
            .ForMember(des => des.VoteContractAddress, opt
                => opt.MapFrom(source => source.ContractAddressList == null ? 
                    string.Empty : MapAddress(source.ContractAddressList.VoteContractAddress)))
            .ForMember(des => des.ElectionContractAddress, opt
                => opt.MapFrom(source => source.ContractAddressList == null ? 
                    string.Empty : MapAddress(source.ContractAddressList.ElectionContractAddress)))
            .ForMember(des => des.GovernanceContractAddress, opt
                => opt.MapFrom(source => source.ContractAddressList == null ? 
                    string.Empty : MapAddress(source.ContractAddressList.GovernanceContractAddress)))
            .ForMember(des => des.TimelockContractAddress, opt
                => opt.MapFrom(source => source.ContractAddressList == null ? 
                    string.Empty : MapAddress(source.ContractAddressList.TimelockContractAddress)))
            ;
        CreateMap<FileInfoContract, FileInfoIndexer>()
            .ForMember(des => des.Uploader, opt
                => opt.MapFrom(source => MapAddress(source.Uploader)))
            .ForMember(des => des.UploadTime, opt
                => opt.MapFrom(source => MapDateTime(source.UploadTime)))
            ;
        CreateMap<FileContract, FileIndexer>();
        CreateMap<Unpaused, DAOIndex>()
            .ForMember(des => des.TreasuryPauseExecutor, opt
                => opt.MapFrom(source => MapAddress(source.Account)))
            ;
        CreateMap<Paused, DAOIndex>()
            .ForMember(des => des.TreasuryPauseExecutor, opt
                => opt.MapFrom(source => MapAddress(source.Account)))
            ;
        CreateMap<PermissionsSet, DAOIndex>()
            .ForMember(des => des.PermissionAddress, opt
                => opt.MapFrom(source => MapAddress(source.Here)))
            .ForMember(des => des.PermissionInfoList, opt
                => opt.MapFrom(source => MapPermissionInfoList(source)))
            ;
        CreateMap<HighCouncilConfigContract, HighCouncilConfigIndexer>();
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
        CreateMap<VoteIndex, VoteInfoDto>()
            .ForMember(des => des.VoterCount, opt
                => opt.MapFrom(source => source.VoterSet.Count));
        CreateMap<VoteRecordIndex, VoteRecordDto>();
        CreateMap<OrganizationIndex, OrganizationInfoDto>()
            .ForMember(des => des.OrganizationMemberCount, opt
                => opt.MapFrom(source => source.OrganizationMemberSet.Count));
        CreateMap<MetadataContract, MetadataIndexer>();
        CreateMap<DAOIndex, DAOInfoDto>();
        CreateMap<MetadataIndexer, MetadataDto>();
        CreateMap<HighCouncilConfigIndexer, HighCouncilConfigDto>();
    }
}