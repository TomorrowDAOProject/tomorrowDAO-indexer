using AeFinder.Sdk.Processor;
using AElf.Contracts.Referendum;
using AElf.Contracts.MultiToken;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Entities.Base;
using TomorrowDAOIndexer.GraphQL.Dto;
using ExecuteTransaction = TomorrowDAOIndexer.Entities.ExecuteTransaction;
using ExecuteTransactionContract = TomorrowDAO.Contracts.Governance.ExecuteTransaction;
using FileInfoIndexer = TomorrowDAOIndexer.Entities.FileInfo;
using FileInfoContract = TomorrowDAO.Contracts.DAO.FileInfo;
using FileIndexer = TomorrowDAOIndexer.Entities.File;
using FileContract = TomorrowDAO.Contracts.DAO.File;
using Transaction = TomorrowDAOIndexer.Entities.Transaction;
using Voted = TomorrowDAO.Contracts.Vote.Voted;

namespace TomorrowDAOIndexer;

public class TomorrowDAOIndexerClientAutoMapperProfile : IndexerMapperBase
{
    public TomorrowDAOIndexerClientAutoMapperProfile()
    {
        CreateMap<DAOIndex, DAOInfoDto>()
            .ForMember(des => des.Metadata, opt => opt.Ignore())
            .ForMember(des => des.ChainId, opt
                => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.BlockHeight))
            ;
        CreateMap<DAOIndex, MetadataDto>();
        CreateMap<LogEventContext, DAOIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<MetadataUpdated, DAOIndex>()
            .ForMember(des => des.Metadata, opt => opt.Ignore())
            .ForMember(des => des.Name, opt
                => opt.MapFrom(source => source.Metadata.Name))
            .ForMember(des => des.LogoUrl, opt
                => opt.MapFrom(source => source.Metadata.LogoUrl))
            .ForMember(des => des.Description, opt
                => opt.MapFrom(source => source.Metadata.Description))
            .ForMember(des => des.SocialMedia, opt
                => opt.MapFrom(source => source.Metadata.SocialMedia))
            ;
        CreateMap<DAOCreated, DAOIndex>()
            .ForMember(des => des.SubsistStatus, opt
                => opt.MapFrom(source => true))
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

        CreateMap<VoteSchemeIndex, VoteSchemeInfoDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            ;
        CreateMap<ElectionIndex, ElectionDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight));

        CreateMap<TreasuryFundIndex, TreasuryFundDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight));

        CreateMap<TreasuryRecordIndex, TreasuryRecordDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight));
        CreateMap<LogEventContext, ProposalIndex>()
            .ForMember(des => des.Transaction, opt => opt.Ignore())
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<LogEventContext, LatestParticipatedIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<GovernanceSchemeIndex, ProposalIndex>();
        CreateMap<ExecuteTransactionContract, ExecuteTransaction>();
        CreateMap<ExecuteTransaction, ExecuteTransactionDto>()
            .ForMember(des => des.Params, opt
                => opt.MapFrom(source => source.Params));
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
        CreateMap<TomorrowDAO.Contracts.Governance.ExecuteTransaction, ExecuteTransaction>()
            .ForMember(des => des.ToAddress, opt => opt.MapFrom(source => MapAddress(source.ToAddress)))
            .ForMember(des => des.Params, opt => opt.MapFrom(source => MapByteStringToBase64(source.Params)));

        CreateMap<ProposalExecuted, ProposalIndex>();
        CreateMap<DAOIndex, ProposalIndex>();
        CreateMap<ProposalIndex, ProposalSyncDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            ;
        CreateMap<LogEventContext, TreasuryFundIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<LogEventContext, TreasuryFundSumIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<LogEventContext, TreasuryCreateIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<TreasuryFundSumIndex, GetDAOAmountRecordDto>()
            .ForMember(des => des.GovernanceToken, opt
                => opt.MapFrom(source => source.Symbol))
            .ForMember(des => des.Amount, opt
                => opt.MapFrom(source => source.AvailableFunds))
            ;
        CreateMap<LogEventContext, TreasuryRecordIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            .ForMember(des => des.CreateTime, opt
                => opt.MapFrom(source => source.Block.BlockTime))
            ;
        CreateMap<TreasuryTransferred, TreasuryRecordIndex>()
            .ForMember(des => des.DaoId, opt
                => opt.MapFrom(source => MapHash(source.DaoId)))
            .ForMember(des => des.TreasuryAddress, opt
                => opt.MapFrom(source => MapAddress(source.TreasuryAddress)))
            .ForMember(des => des.ToAddress, opt
                => opt.MapFrom(source => MapAddress(source.Recipient)))
            .ForMember(des => des.Executor, opt
                => opt.MapFrom(source => MapAddress(source.Executor)))
            .ForMember(des => des.ProposalId, opt
                => opt.MapFrom(source => MapHash(source.ProposalId)))
            ;
        CreateMap<Transferred, TreasuryRecordIndex>()
            .ForMember(des => des.Executor, opt
                => opt.MapFrom(source => MapAddress(source.From)))
            .ForMember(des => des.FromAddress, opt
                => opt.MapFrom(source => MapAddress(source.From)))
            .ForMember(des => des.ToAddress, opt
                => opt.MapFrom(source => MapAddress(source.To)))
            ;
        CreateMap<LogEventContext, ElectionIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<DaoProposalTimePeriodSet, DAOIndex>();
        CreateMap<OrganizationIndex, MemberDto>()
            .ForMember(des => des.ChainId, opt
                => opt.MapFrom(source => source.Metadata.ChainId))
            ;
        CreateMap<LogEventContext, OrganizationIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<FileInfosUploaded, DAOIndex>()
            .ForMember(des => des.Id, opt
                => opt.MapFrom(source => MapHash(source.DaoId)));

        CreateMap<FileInfoContract, FileInfoIndexer>()
            .ForMember(des => des.Uploader, opt
                => opt.MapFrom(source => MapAddress(source.Uploader)))
            .ForMember(des => des.UploadTime, opt
                => opt.MapFrom(source => MapDateTime(source.UploadTime)))
            ;
        CreateMap<FileContract, FileIndexer>();
        CreateMap<HighCouncilEnabled, DAOIndex>()
            .ForMember(des => des.Id, opt
                => opt.MapFrom(source => MapHash(source.DaoId)))
            .ForMember(des => des.IsHighCouncilEnabled, opt
                => opt.MapFrom(source => true))
            .ForMember(des => des.HighCouncilAddress, opt
                => opt.MapFrom(source => MapAddress(source.HighCouncilAddress)))
            .ForMember(des => des.MaxHighCouncilMemberCount, opt
                => opt.MapFrom(source => source.HighCouncilInput.HighCouncilConfig.MaxHighCouncilMemberCount))
            .ForMember(des => des.MaxHighCouncilCandidateCount, opt
                => opt.MapFrom(source => source.HighCouncilInput.HighCouncilConfig.MaxHighCouncilCandidateCount))
            .ForMember(des => des.ElectionPeriod, opt
                => opt.MapFrom(source => source.HighCouncilInput.HighCouncilConfig.ElectionPeriod))
            .ForMember(des => des.StakingAmount, opt
                => opt.MapFrom(source => source.HighCouncilInput.HighCouncilConfig.StakingAmount))
            ;
        CreateMap<ElectionHighCouncilConfigIndex, ElectionHighCouncilConfig>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted))
            ;
        CreateMap<ElectionVotingItemIndex, ElectionVotingItem>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted))
            ;
        CreateMap<GovernanceSchemeIndex, GovernanceSchemeIndexDto>()
            .ForMember(des => des.MinimalRequiredThreshold,
                opt => opt.MapFrom(source => source.MinimalRequiredThreshold))
            .ForMember(des => des.MinimalVoteThreshold,
                opt => opt.MapFrom(source => source.MinimalVoteThreshold))
            .ForMember(des => des.MinimalApproveThreshold,
                opt => opt.MapFrom(source => source.MinimalApproveThreshold))
            .ForMember(des => des.MaximalRejectionThreshold,
                opt => opt.MapFrom(source => source.MaximalRejectionThreshold))
            .ForMember(des => des.MaximalAbstentionThreshold,
                opt => opt.MapFrom(source => source.MaximalAbstentionThreshold))
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId));
        CreateMap<ElectionCandidateElectedIndex, ElectionCandidateElected>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted))
            ;
        CreateMap<CandidateElected, ElectionCandidateElectedIndex>()
            .ForMember(des => des.DaoId, opt
                => opt.MapFrom(source => MapHash(source.DaoId)));
        CreateMap<LogEventContext, ElectionCandidateElectedIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<LogEventContext, GovernanceSchemeIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
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
        CreateMap<VoteItemIndex, VoteItemIndexDto>()
            .ForMember(des => des.VoterCount, opt
                => opt.MapFrom(source => source.VoterCount));
        CreateMap<VoteRecordIndex, VoteRecordDto>()
            .ForMember(des => des.VoteTime, opt
                => opt.MapFrom(source => source.VoteTimestamp))
            .ForMember(des => des.TransactionId, opt
                => opt.MapFrom(source => source.TransactionId))
            .ForMember(des => des.ChainId, opt
                => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            ;
        CreateMap<LogEventContext, VoteSchemeIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<LogEventContext, VoteItemIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<LogEventContext, VoteRecordIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            .ForMember(des => des.TransactionId, opt
                => opt.MapFrom(source => source.Transaction.TransactionId))
            ;
        CreateMap<LogEventContext, VoteWithdrawnIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<LogEventContext, ElectionHighCouncilConfigIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight));
        ;
        CreateMap<LogEventContext, ElectionVotingItemIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<LogEventContext, DaoVoterRecordIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight));
        ;
        CreateMap<VoteSchemeCreated, VoteSchemeIndex>()
            .ForMember(des => des.VoteSchemeId, opt
                => opt.MapFrom(source => MapHash(source.VoteSchemeId)));
        CreateMap<VotingItemRegistered, VoteItemIndex>()
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
                => opt.MapFrom(source => MapDateTime(source.EndTimestamp)))
            ;
        CreateMap<Voted, VoteRecordIndex>()
            .ForMember(des => des.StartTime, opt
                => opt.MapFrom(source => MapDateTime(source.StartTime)))
            .ForMember(des => des.EndTime, opt
                => opt.MapFrom(source => MapDateTime(source.EndTime)))
            .ForMember(des => des.VotingItemId, opt
                => opt.MapFrom(source => MapHash(source.VotingItemId)))
            .ForMember(des => des.Voter, opt
                => opt.MapFrom(source => MapAddress(source.Voter)))
            .ForMember(des => des.VoteTimestamp, opt
                => opt.MapFrom(source => MapDateTime(source.VoteTimestamp)))
            .ForMember(des => des.VoteId, opt
                => opt.MapFrom(source => MapHash(source.VoteId)))
            .ForMember(des => des.DAOId, opt
                => opt.MapFrom(source => MapHash(source.DaoId)));
        ;
        CreateMap<VoteSchemeIndex, VoteSchemeIndexDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted));
        CreateMap<Withdrawn, VoteWithdrawnIndex>()
            .ForMember(des => des.DaoId,
                opt => opt.MapFrom(source => MapHash(source.DaoId)))
            .ForMember(des => des.Voter,
                opt => opt.MapFrom(source => MapAddress(source.Withdrawer)))
            .ForMember(des => des.WithdrawTimestamp,
                opt => opt.MapFrom(source => MapDateTime(source.WithdrawTimestamp)))
            .ForMember(des => des.VotingItemIdList,
                opt => opt.MapFrom(source => MapVotingItemIdList(source.VotingItemIdList)));
        CreateMap<VoteWithdrawnIndex, VoteWithdrawnIndexDto>()
            .ForMember(des => des.BlockHeight,
                opt => opt.MapFrom(source => source.BlockHeight))
            ;
        CreateMap<TomorrowDAO.Contracts.Election.VotingItem, ElectionVotingItemIndex>()
            .ForMember(des => des.VotingItemId, opt => opt.MapFrom(source => MapHash(source.VotingItemId)))
            .ForMember(des => des.RegisterTimestamp,
                opt => opt.MapFrom(source => MapDateTime(source.RegisterTimestamp)))
            .ForMember(des => des.StartTimestamp, opt => opt.MapFrom(source => MapDateTime(source.StartTimestamp)))
            .ForMember(des => des.EndTimestamp, opt => opt.MapFrom(source => MapDateTime(source.EndTimestamp)))
            .ForMember(des => des.CurrentSnapshotStartTimestamp,
                opt => opt.MapFrom(source => MapDateTime(source.CurrentSnapshotStartTimestamp)))
            .ForMember(des => des.Sponsor, opt => opt.MapFrom(source => MapAddress(source.Sponsor)));
        CreateMap<TomorrowDAO.Contracts.Election.HighCouncilConfig, ElectionHighCouncilConfigIndex>();
        CreateMap<DaoVoterRecordIndex, DaoVoterRecordIndexDto>()
            .ForMember(des => des.ChainId,
                opt => opt.MapFrom(source => source.Metadata.ChainId))
            ;
        CreateMap<LogEventContext, NetworkDaoProposalIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<LogEventContext, NetworkDaoProposalReleasedIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight));
        CreateMap<LogEventContext, NetworkDaoProposalVoteRecordIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight));
        CreateMap<LogEventContext, NetworkDaoOrgCreatedIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight));
        CreateMap<LogEventContext, NetworkDaoOrgChangedIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight));
        CreateMap<LogEventContext, NetworkDaoOrgWhiteListChangedIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight));
        CreateMap<LogEventContext, NetworkDaoOrgThresholdChangedIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight));
        CreateMap<LogEventContext, NetworkDaoOrgMemberChangedIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight));
        CreateMap<TransactionInfo, TransactionInfoDto>();
        CreateMap<NetworkDaoOrgChangedIndex, NetworkDaoOrgChangedIndexDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted));
        CreateMap<NetworkDaoOrgCreatedIndex, NetworkDaoOrgCreatedIndexDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted));
        CreateMap<NetworkDaoOrgThresholdChangedIndex, NetworkDaoOrgThresholdChangedIndexDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted));
        CreateMap<NetworkDaoOrgWhiteListChangedIndex, NetworkDaoOrgWhiteListChangedIndexDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted));
        CreateMap<NetworkDaoOrgMemberChangedIndex, NetworkDaoOrgMemberChangedIndexDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted));
        CreateMap<NetworkDaoProposalIndex, NetworkDaoProposalIndexDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted));
        CreateMap<NetworkDaoProposalReleasedIndex, NetworkDaoProposalReleasedIndexDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted));
        CreateMap<NetworkDaoProposalVoteRecordIndex, NetworkDaoProposalVoteRecordIndexDto>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted));
        CreateMap<NetworkDaoProposalIndex, NetworkDaoProposal>()
            .ForMember(des => des.ChainId, opt => opt.MapFrom(source => source.Metadata.ChainId))
            .ForMember(des => des.BlockHash, opt => opt.MapFrom(source => source.Metadata.Block.BlockHash))
            .ForMember(des => des.BlockHeight, opt => opt.MapFrom(source => source.Metadata.Block.BlockHeight))
            .ForMember(des => des.BlockTime, opt => opt.MapFrom(source => source.Metadata.Block.BlockTime))
            .ForMember(des => des.IsDeleted, opt => opt.MapFrom(source => source.Metadata.IsDeleted));
        CreateMap<AElf.Standards.ACS3.ProposalCreated, NetworkDaoProposalIndex>()
            .ForMember(des => des.ProposalId, opt => opt.MapFrom(source => MapHash(source.ProposalId)))
            .ForMember(des => des.OrganizationAddress,
                opt => opt.MapFrom(source => MapAddress(source.OrganizationAddress)));
        CreateMap<AElf.Standards.ACS3.ProposalReleased, NetworkDaoProposalReleasedIndex>()
            .ForMember(des => des.ProposalId, opt => opt.MapFrom(source => MapHash(source.ProposalId)))
            .ForMember(des => des.OrganizationAddress,
                opt => opt.MapFrom(source => MapAddress(source.OrganizationAddress)));
        CreateMap<AElf.Standards.ACS3.ReceiptCreated, NetworkDaoProposalVoteRecordIndex>()
            .ForMember(des => des.ProposalId, opt => opt.MapFrom(source => MapHash(source.ProposalId)))
            .ForMember(des => des.Address, opt => opt.MapFrom(source => MapAddress(source.Address)))
            .ForMember(des => des.ReceiptType, opt => opt.MapFrom(source => MapReceiptType(source.ReceiptType)))
            .ForMember(des => des.OrganizationAddress, opt => opt.MapFrom(source => MapAddress(source.OrganizationAddress)));
        CreateMap<ReferendumReceiptCreated, NetworkDaoProposalVoteRecordIndex>()
            .ForMember(des => des.ProposalId, opt => opt.MapFrom(source => MapHash(source.ProposalId)))
            .ForMember(des => des.Address, opt => opt.MapFrom(source => MapAddress(source.Address)))
            .ForMember(des => des.ReceiptType, opt => opt.MapFrom(source => MapReceiptType(source.ReceiptType)))
            .ForMember(des => des.OrganizationAddress, opt => opt.MapFrom(source => MapAddress(source.OrganizationAddress)))
            .ForMember(des => des.Time, opt => opt.MapFrom(source => MapDateTime(source.Time)));
        CreateMap<AElf.Standards.ACS3.OrganizationCreated, NetworkDaoOrgCreatedIndex>()
            .ForMember(des => des.OrganizationAddress, opt => opt.MapFrom(source => MapAddress(source.OrganizationAddress)));
        CreateMap<AeFinder.Sdk.Processor.Transaction, Transaction>();
        CreateMap<AElf.Standards.ACS3.OrganizationWhiteListChanged, NetworkDaoOrgWhiteListChangedIndex>()
            .ForMember(des => des.OrganizationAddress, opt => opt.MapFrom(source => MapAddress(source.OrganizationAddress)))
            .ForMember(des => des.ProposerWhiteList, opt => opt.MapFrom(source => MapAddressList(source.ProposerWhiteList.Proposers)));
        CreateMap<AElf.Standards.ACS3.OrganizationThresholdChanged, NetworkDaoOrgThresholdChangedIndex>()
            .ForMember(des => des.OrganizationAddress, opt => opt.MapFrom(source => MapAddress(source.OrganizationAddress)))
            .ForMember(des => des.MinimalApprovalThreshold, opt => opt.MapFrom(source => source.ProposerReleaseThreshold.MinimalApprovalThreshold))
            .ForMember(des => des.MaximalRejectionThreshold, opt => opt.MapFrom(source => source.ProposerReleaseThreshold.MaximalRejectionThreshold))
            .ForMember(des => des.MaximalAbstentionThreshold, opt => opt.MapFrom(source => source.ProposerReleaseThreshold.MaximalAbstentionThreshold))
            .ForMember(des => des.MinimalVoteThreshold, opt => opt.MapFrom(source => source.ProposerReleaseThreshold.MinimalVoteThreshold));
        CreateMap<AElf.Contracts.Association.MemberAdded, NetworkDaoOrgMemberChangedIndex>()
            .ForMember(des => des.OrganizationAddress, opt => opt.MapFrom(source => MapAddress(source.OrganizationAddress)))
            .ForMember(des => des.AddedAddress, opt => opt.MapFrom(source => MapAddress(source.Member)));
        CreateMap<AElf.Contracts.Association.MemberRemoved, NetworkDaoOrgMemberChangedIndex>()
            .ForMember(des => des.OrganizationAddress, opt => opt.MapFrom(source => MapAddress(source.OrganizationAddress)))
            .ForMember(des => des.RemovedAddress, opt => opt.MapFrom(source => MapAddress(source.Member)));
        CreateMap<AElf.Contracts.Association.MemberChanged, NetworkDaoOrgMemberChangedIndex>()
            .ForMember(des => des.OrganizationAddress, opt => opt.MapFrom(source => MapAddress(source.OrganizationAddress)))
            .ForMember(des => des.AddedAddress, opt => opt.MapFrom(source => MapAddress(source.NewMember)))
            .ForMember(des => des.RemovedAddress, opt => opt.MapFrom(source => MapAddress(source.OldMember)));
        CreateMap<AElf.Standards.ACS3.ReceiptCreated, ReferendumReceiptCreated>();
        CreateMap<DAOIndex, GetDAOAmountRecordDto>()
            .ForMember(des => des.Amount, opt => opt.MapFrom(source => source.VoteAmount - source.WithdrawAmount))
            ;
        CreateMap<UserBalanceIndex, UserBalanceDto>()
            .ForMember(des => des.ChainId, opt
                => opt.MapFrom(source => source.Metadata.ChainId))
            ;
        CreateMap<LogEventContext, UserBalanceIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
        CreateMap<ResourceTokenIndex, ResourceTokenDto>();
        CreateMap<LogEventContext, ResourceTokenIndex>()
            .ForMember(des => des.BlockHeight, opt
                => opt.MapFrom(source => source.Block.BlockHeight))
            ;
    }
}