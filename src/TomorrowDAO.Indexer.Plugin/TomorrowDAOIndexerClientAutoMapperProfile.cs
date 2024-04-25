using AElfIndexer.Client.Handlers;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using ExecuteTransaction = TomorrowDAO.Indexer.Plugin.Entities.ExecuteTransaction;
using ExecuteTransactionContract = TomorrowDAO.Contracts.Governance.ExecuteTransaction;
using HighCouncilConfigContract = TomorrowDAO.Contracts.DAO.HighCouncilConfig;
using HighCouncilConfigIndexer = TomorrowDAO.Indexer.Plugin.Entities.HighCouncilConfig;
using MetadataContract = TomorrowDAO.Contracts.DAO.Metadata;
using MetadataIndexer = TomorrowDAO.Indexer.Plugin.Entities.Metadata;
using FileInfoIndexer = TomorrowDAO.Indexer.Plugin.Entities.FileInfo;
using FileInfoContract = TomorrowDAO.Contracts.DAO.FileInfo;
using FileIndexer = TomorrowDAO.Indexer.Plugin.Entities.File;
using FileContract = TomorrowDAO.Contracts.DAO.File;
using GovernanceSchemeThreshold = TomorrowDAO.Indexer.Plugin.Entities.GovernanceSchemeThreshold;

namespace TomorrowDAO.Indexer.Plugin;

public class TomorrowDAOIndexerClientAutoMapperProfile : IndexerMapperBase
{
    public TomorrowDAOIndexerClientAutoMapperProfile()
    {
        CreateMap<VoteSchemeIndex, VoteSchemeInfoDto>();
        CreateMap<ElectionIndex, ElectionDto>();
        CreateMap<TreasuryFundIndex, TreasuryFundDto>();
        CreateMap<TreasuryRecordIndex, TreasuryRecordDto>();
        CreateMap<LogEventContext, ProposalIndex>();
        CreateMap<GovernanceSchemeIndex, ProposalIndex>();
        CreateMap<ExecuteTransactionContract, ExecuteTransaction>();
        CreateMap<ExecuteTransaction, ExecuteTransactionDto>()
            .ForMember(des => des.Params, opt
                => opt.MapFrom(source => MapToJsonString(source.Params)));
        CreateMap<ProposalCreated, ProposalIndex>()
            .ForMember(des => des.Id, opt
                => opt.MapFrom(source => MapHash(source.ProposalId)))
            .ForMember(des => des.DAOId, opt
                => opt.MapFrom(source => MapHash(source.DaoId)))
            .ForMember(des => des.ProposalId, opt
                => opt.MapFrom(source => MapHash(source.ProposalId)))
            .ForMember(des => des.ActiveStartTime, opt
                => opt.MapFrom(source => MapDateTime(source.ActiveStartTime)))
            .ForMember(des => des.ActiveEndTime, opt
                => opt.MapFrom(source => MapDateTime(source.ActiveEndTime)))
            .ForMember(des => des.ExecuteStartTime, opt
                => opt.MapFrom(source => MapDateTime(source.ExecuteStartTime)))
            .ForMember(des => des.ExecuteEndTime, opt
                => opt.MapFrom(source => MapDateTime(source.ExecuteEndTime)))
            .ForMember(des => des.Proposer, opt
                => opt.MapFrom(source => MapAddress(source.Proposer)))
            .ForMember(des => des.SchemeAddress, opt
                => opt.MapFrom(source => MapAddress(source.SchemeAddress)))
            .ForMember(des => des.VoteSchemeId, opt
                => opt.MapFrom(source => MapHash(source.VoteSchemeId)))
            .ForMember(des => des.VetoProposalId, opt
                => opt.MapFrom(source => MapHash(source.VetoProposalId)))
            ;

        CreateMap<ProposalExecuted, ProposalIndex>();
        CreateMap<DAOIndex, ProposalIndex>();
        CreateMap<ProposalIndex, ProposalSyncDto>()
            .ForMember(des => des.ProposalStatus, opt
                => opt.MapFrom(source => (int)source.ProposalStatus))
            .ForMember(des => des.ProposalStage, opt
                => opt.MapFrom(source => (int)source.ProposalStage))
            .ForMember(des => des.GovernanceMechanism, opt
                => opt.MapFrom(source => (int)source.GovernanceMechanism))
            ;
        CreateMap<LogEventContext, DAOIndex>();
        CreateMap<LogEventContext, TreasuryFundIndex>();
        CreateMap<LogEventContext, TreasuryRecordIndex>();
        CreateMap<LogEventContext, ElectionIndex>();
        CreateMap<DaoProposalTimePeriodSet, DAOIndex>();
        CreateMap<DAOCreated, DAOIndex>()
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
        CreateMap<HighCouncilEnabled, DAOIndex>()
            .ForMember(des => des.HighCouncilAddress, opt
                => opt.MapFrom(source => MapAddress(source.HighCouncilAddress)))
            .ForMember(des => des.HighCouncilConfig, opt
                => opt.MapFrom(source =>
                    source.HighCouncilInput == null ? null : source.HighCouncilInput.HighCouncilConfig))
            ;
        CreateMap<HighCouncilConfigContract, HighCouncilConfigIndexer>();
        CreateMap<LogEventContext, GovernanceSchemeIndex>();
        CreateMap<GovernanceSchemeAdded, GovernanceSchemeIndex>()
            .ForMember(des => des.SchemeId, opt
                => opt.MapFrom(source => MapHash(source.SchemeId)))
            .ForMember(des => des.DAOId, opt
                => opt.MapFrom(source => MapHash(source.DaoId)))
            .ForMember(des => des.SchemeAddress, opt
                => opt.MapFrom(source => MapAddress(source.SchemeAddress)))
            .ForMember(des => des.MinimalRequiredThreshold,
                opt => opt.MapFrom(source => source.SchemeThreshold.MinimalRequiredThreshold))
            .ForMember(des => des.MinimalVoteThreshold,
                opt => opt.MapFrom(source => source.SchemeThreshold.MinimalVoteThreshold))
            .ForMember(des => des.MinimalApproveThreshold,
                opt => opt.MapFrom(source => source.SchemeThreshold.MinimalApproveThreshold))
            .ForMember(des => des.MaximalRejectionThreshold,
                opt => opt.MapFrom(source => source.SchemeThreshold.MaximalRejectionThreshold))
            .ForMember(des => des.MaximalAbstentionThreshold,
                opt => opt.MapFrom(source => source.SchemeThreshold.MaximalAbstentionThreshold));
        CreateMap<GovernanceSchemeIndex, GovernanceSchemeIndexDto>();
        CreateMap<GovernanceSchemeThreshold, GovernanceSchemeThresholdDto>();
        CreateMap<VoteIndex, VoteInfoDto>()
            .ForMember(des => des.VoterCount, opt
                => opt.MapFrom(source => source.VoterSet.Count));
        CreateMap<VoteRecordIndex, VoteRecordDto>();
        CreateMap<MetadataContract, MetadataIndexer>();
        CreateMap<DAOIndex, DAOInfoDto>();
        CreateMap<MetadataIndexer, MetadataDto>();
        CreateMap<HighCouncilConfigIndexer, HighCouncilConfigDto>();
        CreateMap<LogEventContext, VoteSchemeIndex>();
        CreateMap<LogEventContext, VoteIndex>();
        CreateMap<LogEventContext, VoteRecordIndex>();
        CreateMap<VoteSchemeCreated, VoteSchemeIndex>()
            .ForMember(des => des.VoteSchemeId, opt
                => opt.MapFrom(source => MapHash(source.VoteSchemeId)));
        CreateMap<VotingItemRegistered, VoteIndex>()
            .ForMember(des => des.DAOId, opt
                => opt.MapFrom(source => MapHash(source.DaoId)))
            .ForMember(des => des.VotingItemId, opt
                => opt.MapFrom(source => MapHash(source.VotingItemId)))
            .ForMember(des => des.VoteSchemeId, opt
                => opt.MapFrom(source => MapHash(source.SchemeId)))
            .ForMember(des => des.RegisterTime, opt
                => opt.MapFrom(source => MapDateTime(source.RegisterTimestamp)))
            .ForMember(des => des.StartTime, opt
                => opt.MapFrom(source => MapDateTime(source.StartTimestamp)))
            .ForMember(des => des.EndTime, opt
                => opt.MapFrom(source => MapDateTime(source.EndTimestamp)));
        CreateMap<Voted, VoteRecordIndex>()
            .ForMember(des => des.VotingItemId, opt
                => opt.MapFrom(source => MapHash(source.VotingItemId)))
            .ForMember(des => des.Voter, opt
                => opt.MapFrom(source => MapAddress(source.Voter)))
            .ForMember(des => des.VoteTime, opt
                => opt.MapFrom(source => MapDateTime(source.VoteTimestamp)));
    }
}