using AElfIndexer.Client.Handlers;
using AutoMapper;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;

namespace TomorrowDAO.Indexer.Plugin;

public class TomorrowDAOIndexerClientAutoMapperProfile : Profile
{
    public TomorrowDAOIndexerClientAutoMapperProfile()
    {
        CreateMap<LogEventContext, ProposalIndex>();
        CreateMap<ProposalCreated, ProposalIndex>()
            .ForMember(des => des.DaoId, opt
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
    }
}