using AeFinder.App.TestBase;
using Microsoft.Extensions.DependencyInjection;
using TomorrowDAOIndexer.Processors.DAO;
using TomorrowDAOIndexer.Processors.GovernanceScheme;
using TomorrowDAOIndexer.Processors.NetworkDao.Association;
using TomorrowDAOIndexer.Processors.NetworkDao.Parliament;
using TomorrowDAOIndexer.Processors.NetworkDao.Referendum;
using TomorrowDAOIndexer.Processors.Proposal;
using TomorrowDAOIndexer.Processors.Token;
using TomorrowDAOIndexer.Processors.TokenConverter;
using TomorrowDAOIndexer.Processors.Treasury;
using TomorrowDAOIndexer.Processors.Vote;
using Volo.Abp.Modularity;
using Election = TomorrowDAOIndexer.Processors.Election;

namespace TomorrowDAOIndexer;

[DependsOn(
    typeof(AeFinderAppTestBaseModule),
    typeof(TomorrowDAOIndexerModule))]
public class TomorrowDAOIndexerTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AeFinderAppEntityOptions>(options => { options.AddTypes<TomorrowDAOIndexerModule>(); });
        
        // TokenConverter
        context.Services.AddSingleton<TokenSoldProcessor>();
        context.Services.AddSingleton<TokenBoughtProcessor>();
        
        //  DAO
        context.Services.AddSingleton<DAOCreatedProcessor>();
        context.Services.AddSingleton<FileInfosRemovedProcessor>();
        context.Services.AddSingleton<FileInfosUploadedProcessor>();
        context.Services.AddSingleton<HighCouncilDisabledProcessor>();
        context.Services.AddSingleton<HighCouncilEnabledProcessor>();
        context.Services.AddSingleton<SubsistStatusSetProcessor>();
        context.Services.AddSingleton<MetadataUpdatedProcessor>();
        context.Services.AddSingleton<MemberAddedProcessor>();
        context.Services.AddSingleton<MemberRemovedProcessor>();
        
        // GovernanceScheme
        context.Services.AddSingleton<GovernanceSchemeAddedProcessor>();
        context.Services.AddSingleton<GovernanceSchemeThresholdRemovedProcessor>();
        context.Services.AddSingleton<GovernanceSchemeThresholdUpdatedProcessor>();
        context.Services.AddSingleton<GovernanceTokenSetProcessor>();
        
        // proposal
        context.Services.AddSingleton<ProposalCreatedProcessor>();
        context.Services.AddSingleton<ProposalExecutedProcessor>();
        context.Services.AddSingleton<ProposalVetoedProcessor>();
        context.Services.AddSingleton<DAOProposalTimePeriodSetProcessor>();
        
        // vote
        context.Services.AddSingleton<VoteSchemeCreatedProcessor>();
        context.Services.AddSingleton<VotingItemRegisteredProcessor>();
        context.Services.AddSingleton<VotedProcessor>();
        context.Services.AddSingleton<VoteWithdrawnProcessor>();
        
        // treasury
        context.Services.AddSingleton<TransferredProcessor>();
        context.Services.AddSingleton<TreasuryCreatedProcessor>();
        context.Services.AddSingleton<TreasuryTransferredProcessor>();
        
        // election
        context.Services.AddSingleton<Election.CandidateAddedProcessor>();
        context.Services.AddSingleton<Election.CandidateAddressReplacedProcessor>();
        context.Services.AddSingleton<Election.CandidateInfoUpdatedProcessor>();
        context.Services.AddSingleton<Election.CandidateRemovedProcessor>();
        context.Services.AddSingleton<Election.VotedProcessor>();
        context.Services.AddSingleton<Election.ElectionVotingEventRegisteredProcessor>();
        context.Services.AddSingleton<Election.HighCouncilAddedProcessor>();
        context.Services.AddSingleton<Election.HighCouncilRemovedProcessor>();
        context.Services.AddSingleton<Election.CandidateElectedProcessor>();
        
        //Network DAO
        context.Services.AddSingleton<AssociationProposalCreatedProcessor>();
        context.Services.AddSingleton<ReferendumProposalCreatedProcessor>();
        context.Services.AddSingleton<ParliamentProposalCreatedProcessor>();
        // context.Services.AddSingleton<NetworkDaoTransferredProcessor>();
        
        // Transaction
        // context.Services.AddSingleton<TransactionProcessor>();
    }
}