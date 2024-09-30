// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: referendum_contract.proto
// </auto-generated>
// Original file comments:
// *
// Referendum contract.
#pragma warning disable 0414, 1591
#region Designer generated code

using System.Collections.Generic;
using aelf = global::AElf.CSharp.Core;

namespace AElf.Contracts.Referendum {

  #region Events
  public partial class ReferendumReceiptCreated : aelf::IEvent<ReferendumReceiptCreated>
  {
    public global::System.Collections.Generic.IEnumerable<ReferendumReceiptCreated> GetIndexed()
    {
      return new List<ReferendumReceiptCreated>
      {
      new ReferendumReceiptCreated
      {
        OrganizationAddress = OrganizationAddress
      },
      };
    }

    public ReferendumReceiptCreated GetNonIndexed()
    {
      return new ReferendumReceiptCreated
      {
        ProposalId = ProposalId,
        Address = Address,
        Symbol = Symbol,
        Amount = Amount,
        ReceiptType = ReceiptType,
        Time = Time,
      };
    }
  }

  #endregion
}
#endregion

