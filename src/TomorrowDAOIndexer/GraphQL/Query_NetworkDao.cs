using System.Linq.Expressions;
using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.GraphQL.Dto;
using TomorrowDAOIndexer.GraphQL.Input;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Obsolete]
    [Name("getNetworkDaoProposals")]
    public static async Task<NetworkDaoProposalsDto> GetNetworkDaoProposalsAsync(
        [FromServices] IReadOnlyRepository<NetworkDaoProposalIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetNetworkDaoProposalsInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (input.ProposalType != NetworkDaoProposalType.All)
        {
            queryable = queryable.Where(a => a.OrgType == (NetworkDaoOrgType)(int)input.ProposalType);
        }
        if (!input.ProposalIds.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.ProposalIds.Select(proposalId => (Expression<Func<NetworkDaoProposalIndex, bool>>)(o => o.ProposalId == proposalId))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        return new NetworkDaoProposalsDto
        {
            TotalCount = count,
            Items = objectMapper.Map<List<NetworkDaoProposalIndex>, List<NetworkDaoProposal>>(queryable.ToList())
        };
    }
    
    //Query organization change records
    [Name("getNetworkDaoOrgChangedIndex")]
    public static async Task<NetworkDaoOrgChangedPageResultDto> GetNetworkDaoOrgChangedIndexAsync(
        [FromServices] IReadOnlyRepository<NetworkDaoOrgChangedIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetNetworkDaoDataChangedIndexInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (input.OrgType != NetworkDaoOrgType.All)
        {
            queryable = queryable.Where(a => a.OrgType == input.OrgType);
        }
        if (!input.OrgAddresses.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.OrgAddresses.Select(orgAddress => (Expression<Func<NetworkDaoOrgChangedIndex, bool>>)(o => o.OrganizationAddress == orgAddress))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        
        return new NetworkDaoOrgChangedPageResultDto
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoOrgChangedIndex>, List<NetworkDaoOrgChangedIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the record of organization creation
    [Name("getNetworkDaoOrgCreatedIndex")]
    public static async Task<NetworkDaoOrgCreatedPageResultDto> GetNetworkDaoOrgCreatedIndexAsync(
        [FromServices] IReadOnlyRepository<NetworkDaoOrgCreatedIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetNetworkDaoDataCreatedIndexInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (input.OrgType != NetworkDaoOrgType.All)
        {
            queryable = queryable.Where(a => a.OrgType == input.OrgType);
        }
        if (!input.OrgAddresses.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.OrgAddresses.Select(orgAddress => (Expression<Func<NetworkDaoOrgCreatedIndex, bool>>)(o => o.OrganizationAddress == orgAddress))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        
        return new NetworkDaoOrgCreatedPageResultDto
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoOrgCreatedIndex>, List<NetworkDaoOrgCreatedIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the organization Threshold change record
    [Name("getNetworkDaoOrgThresholdChangedIndex")]
    public static async Task<NetworkDaoOrgThresholdChangedPageResultDto> GetNetworkDaoOrgThresholdChangedIndexAsync(
        [FromServices] IReadOnlyRepository<NetworkDaoOrgThresholdChangedIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetNetworkDaoDataThresholdChangedIndexInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (input.OrgType != NetworkDaoOrgType.All)
        {
            queryable = queryable.Where(a => a.OrgType == input.OrgType);
        }
        if (!input.OrgAddresses.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.OrgAddresses.Select(orgAddress => (Expression<Func<NetworkDaoOrgThresholdChangedIndex, bool>>)(o => o.OrganizationAddress == orgAddress))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        
        return new NetworkDaoOrgThresholdChangedPageResultDto
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoOrgThresholdChangedIndex>, List<NetworkDaoOrgThresholdChangedIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the change records of the organization's whitelist
    [Name("getNetworkDaoOrgWhiteListChangedIndex")]
    public static async Task<NetworkDaoOrgWhiteListChangedPageResultDto> GetNetworkDaoOrgWhiteListChangedIndexAsync(
        [FromServices] IReadOnlyRepository<NetworkDaoOrgWhiteListChangedIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetNetworkDaoDataWhiteListChangedIndexInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (input.OrgType != NetworkDaoOrgType.All)
        {
            queryable = queryable.Where(a => a.OrgType == input.OrgType);
        }
        if (!input.OrgAddresses.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.OrgAddresses.Select(orgAddress => (Expression<Func<NetworkDaoOrgWhiteListChangedIndex, bool>>)(o => o.OrganizationAddress == orgAddress))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        
        return new NetworkDaoOrgWhiteListChangedPageResultDto
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoOrgWhiteListChangedIndex>, List<NetworkDaoOrgWhiteListChangedIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the change records of organization members
    [Name("getNetworkDaoOrgMemberChangedIndex")]
    public static async Task<NetworkDaoOrgMemberChangedPageResultDto> GetNetworkDaoOrgMemberChangedIndexAsync(
        [FromServices] IReadOnlyRepository<NetworkDaoOrgMemberChangedIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetNetworkDaoDataMemberChangedIndexInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (input.OrgType != NetworkDaoOrgType.All)
        {
            queryable = queryable.Where(a => a.OrgType == input.OrgType);
        }
        if (input.ChangeType != OrgMemberChangeTypeEnum.All)
        {
            queryable = queryable.Where(a => a.ChangeType == input.ChangeType);
        }
        if (!input.OrgAddresses.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.OrgAddresses.Select(orgAddress => (Expression<Func<NetworkDaoOrgMemberChangedIndex, bool>>)(o => o.OrganizationAddress == orgAddress))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        
        return new NetworkDaoOrgMemberChangedPageResultDto
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoOrgMemberChangedIndex>, List<NetworkDaoOrgMemberChangedIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the record of proposal creation
    [Name("getNetworkDaoProposalIndex")]
    public static async Task<NetworkDaoProposalPageResultDto> GetNetworkDaoProposalIndexAsync(
        [FromServices] IReadOnlyRepository<NetworkDaoProposalIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetNetworkDaoProposalIndexInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (input.OrgType != NetworkDaoOrgType.All)
        {
            queryable = queryable.Where(a => a.OrgType == input.OrgType);
        }
        if (!input.Title.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Title == input.Title);
        }
        if (!input.ProposalIds.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.ProposalIds.Select(proposalId => (Expression<Func<NetworkDaoProposalIndex, bool>>)(o => o.ProposalId == proposalId))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        if (!input.OrgAddresses.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.OrgAddresses.Select(orgAddress => (Expression<Func<NetworkDaoProposalIndex, bool>>)(o => o.OrganizationAddress == orgAddress))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        if (!input.ContractNames.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.ContractNames.Select(contractName => (Expression<Func<NetworkDaoProposalIndex, bool>>)(o => o.TransactionInfo.To.Contains(contractName)))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        if (!input.MethodNames.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.MethodNames.Select(methodName => (Expression<Func<NetworkDaoProposalIndex, bool>>)(o => o.TransactionInfo.MethodName.Contains(methodName)))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        
        return new NetworkDaoProposalPageResultDto
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoProposalIndex>, List<NetworkDaoProposalIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the proposal release record
    [Name("getNetworkDaoProposalReleasedIndex")]
    public static async Task<NetworkDaoProposalReleasedPageResultDto> GetNetworkDaoProposalReleasedIndexAsync(
        [FromServices] IReadOnlyRepository<NetworkDaoProposalReleasedIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetNetworkDaoProposalReleasedIndexInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (input.OrgType != NetworkDaoOrgType.All)
        {
            queryable = queryable.Where(a => a.OrgType == input.OrgType);
        }
        if (!input.Title.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Title == input.Title);
        }
        if (!input.ProposalIds.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.ProposalIds.Select(proposalId => (Expression<Func<NetworkDaoProposalReleasedIndex, bool>>)(o => o.ProposalId == proposalId))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        if (!input.OrgAddresses.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.OrgAddresses.Select(orgAddress => (Expression<Func<NetworkDaoProposalReleasedIndex, bool>>)(o => o.OrganizationAddress == orgAddress))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        
        return new NetworkDaoProposalReleasedPageResultDto
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoProposalReleasedIndex>, List<NetworkDaoProposalReleasedIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the voting record of the proposal
    [Name("getNetworkDaoProposalVoteRecordIndex")]
    public static async Task<NetworkDaoProposalVoteRecordPageResultDto> GetNetworkDaoProposalVoteRecordIndexAsync(
        [FromServices] IReadOnlyRepository<NetworkDaoProposalVoteRecordIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetNetworkDaoProposalVoteRecordIndexInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (input.OrgType != NetworkDaoOrgType.All)
        {
            queryable = queryable.Where(a => a.OrgType == input.OrgType);
        }
        if (!input.ProposalIds.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.ProposalIds.Select(proposalId => (Expression<Func<NetworkDaoProposalVoteRecordIndex, bool>>)(o => o.ProposalId == proposalId))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        if (!input.OrgAddresses.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.OrgAddresses.Select(orgAddress => (Expression<Func<NetworkDaoProposalVoteRecordIndex, bool>>)(o => o.OrganizationAddress == orgAddress))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        if (input.ReceiptType != ReceiptTypeEnum.All)
        {
            queryable = queryable.Where(a => a.ReceiptType == input.ReceiptType);
        }
        
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        
        return new NetworkDaoProposalVoteRecordPageResultDto
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoProposalVoteRecordIndex>, List<NetworkDaoProposalVoteRecordIndexDto>>(queryable.ToList())
        };
    }
    
    [Name("getResourceTokenList")]
    public static async Task<List<ResourceTokenDto>> GetResourceTokenListAsync(
        [FromServices] IReadOnlyRepository<ResourceTokenIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetChainBlockHeightInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        return objectMapper.Map<List<ResourceTokenIndex>, List<ResourceTokenDto>>(queryable.ToList());
    }
}