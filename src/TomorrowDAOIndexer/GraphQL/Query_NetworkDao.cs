using System.Linq.Expressions;
using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.GraphQL.Dto;
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
    public static async Task<PageResultDto<NetworkDaoOrgChangedIndexDto>> GetNetworkDaoOrgChangedIndexAsync(
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
        
        return new PageResultDto<NetworkDaoOrgChangedIndexDto>()
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoOrgChangedIndex>, List<NetworkDaoOrgChangedIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the record of organization creation
    [Name("getNetworkDaoOrgCreatedIndex")]
    public static async Task<PageResultDto<NetworkDaoOrgCreatedIndexDto>> GetNetworkDaoOrgCreatedIndexAsync(
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
        
        return new PageResultDto<NetworkDaoOrgCreatedIndexDto>()
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoOrgCreatedIndex>, List<NetworkDaoOrgCreatedIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the organization Threshold change record
    [Name("getNetworkDaoOrgThresholdChangedIndex")]
    public static async Task<PageResultDto<NetworkDaoOrgThresholdChangedIndexDto>> GetNetworkDaoOrgThresholdChangedIndexAsync(
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
        
        return new PageResultDto<NetworkDaoOrgThresholdChangedIndexDto>()
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoOrgThresholdChangedIndex>, List<NetworkDaoOrgThresholdChangedIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the change records of the organization's whitelist
    [Name("getNetworkDaoOrgWhiteListChangedIndex")]
    public static async Task<PageResultDto<NetworkDaoOrgWhiteListChangedIndexDto>> GetNetworkDaoOrgWhiteListChangedIndexAsync(
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
        
        return new PageResultDto<NetworkDaoOrgWhiteListChangedIndexDto>()
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoOrgWhiteListChangedIndex>, List<NetworkDaoOrgWhiteListChangedIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the change records of organization members
    [Name("getNetworkDaoOrgMemberChangedIndex")]
    public static async Task<PageResultDto<NetworkDaoOrgMemberChangedIndexDto>> GetNetworkDaoOrgMemberChangedIndexAsync(
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
        
        return new PageResultDto<NetworkDaoOrgMemberChangedIndexDto>()
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoOrgMemberChangedIndex>, List<NetworkDaoOrgMemberChangedIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the record of proposal creation
    [Name("getNetworkDaoProposalIndex")]
    public static async Task<PageResultDto<NetworkDaoProposalIndexDto>> GetNetworkDaoProposalIndexAsync(
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
                input.OrgAddresses.Select(proposalId => (Expression<Func<NetworkDaoProposalIndex, bool>>)(o => o.ProposalId == proposalId))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        if (!input.OrgAddresses.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.OrgAddresses.Select(orgAddress => (Expression<Func<NetworkDaoProposalIndex, bool>>)(o => o.OrganizationAddress == orgAddress))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        
        return new PageResultDto<NetworkDaoProposalIndexDto>()
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoProposalIndex>, List<NetworkDaoProposalIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the proposal release record
    [Name("getNetworkDaoProposalReleasedIndex")]
    public static async Task<PageResultDto<NetworkDaoProposalReleasedIndexDto>> GetNetworkDaoProposalReleasedIndexAsync(
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
                input.OrgAddresses.Select(proposalId => (Expression<Func<NetworkDaoProposalReleasedIndex, bool>>)(o => o.ProposalId == proposalId))
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
        
        return new PageResultDto<NetworkDaoProposalReleasedIndexDto>()
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoProposalReleasedIndex>, List<NetworkDaoProposalReleasedIndexDto>>(queryable.ToList())
        };
    }
    
    //Query the voting record of the proposal
    [Name("getNetworkDaoProposalVoteRecordIndex")]
    public static async Task<PageResultDto<NetworkDaoProposalVoteRecordIndexDto>> GetNetworkDaoProposalVoteRecordIndexAsync(
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
                input.OrgAddresses.Select(proposalId => (Expression<Func<NetworkDaoProposalVoteRecordIndex, bool>>)(o => o.ProposalId == proposalId))
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
        
        return new PageResultDto<NetworkDaoProposalVoteRecordIndexDto>()
        {
            TotalCount = count,
            Data = objectMapper.Map<List<NetworkDaoProposalVoteRecordIndex>, List<NetworkDaoProposalVoteRecordIndexDto>>(queryable.ToList())
        };
    }
}