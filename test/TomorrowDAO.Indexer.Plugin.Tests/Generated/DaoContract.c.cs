// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protobuf/contract/dao_contract.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using System.Collections.Generic;
using aelf = global::AElf.CSharp.Core;

namespace TomorrowDAO.Contracts.DAO {

  #region Events
  public partial class DAOCreated : aelf::IEvent<DAOCreated>
  {
    public global::System.Collections.Generic.IEnumerable<DAOCreated> GetIndexed()
    {
      return new List<DAOCreated>
      {
      };
    }

    public DAOCreated GetNonIndexed()
    {
      return new DAOCreated
      {
        Metadata = Metadata,
        MetadataAdmin = MetadataAdmin,
        GovernanceToken = GovernanceToken,
        GovernanceSchemeId = GovernanceSchemeId,
        GovernanceSchemeThreshold = GovernanceSchemeThreshold,
        IsHighCouncilEnabled = IsHighCouncilEnabled,
        HighCouncilConfig = HighCouncilConfig,
        FileInfoList = FileInfoList,
        IsTreasuryContractNeeded = IsTreasuryContractNeeded,
        IsVoteContractNeeded = IsVoteContractNeeded,
        DaoId = DaoId,
        Creator = Creator,
      };
    }
  }

  public partial class SubsistStatusSet : aelf::IEvent<SubsistStatusSet>
  {
    public global::System.Collections.Generic.IEnumerable<SubsistStatusSet> GetIndexed()
    {
      return new List<SubsistStatusSet>
      {
      };
    }

    public SubsistStatusSet GetNonIndexed()
    {
      return new SubsistStatusSet
      {
        DaoId = DaoId,
        Status = Status,
      };
    }
  }

  public partial class TreasuryContractSet : aelf::IEvent<TreasuryContractSet>
  {
    public global::System.Collections.Generic.IEnumerable<TreasuryContractSet> GetIndexed()
    {
      return new List<TreasuryContractSet>
      {
      };
    }

    public TreasuryContractSet GetNonIndexed()
    {
      return new TreasuryContractSet
      {
        DaoId = DaoId,
        TreasuryContract = TreasuryContract,
      };
    }
  }

  public partial class VoteContractSet : aelf::IEvent<VoteContractSet>
  {
    public global::System.Collections.Generic.IEnumerable<VoteContractSet> GetIndexed()
    {
      return new List<VoteContractSet>
      {
      };
    }

    public VoteContractSet GetNonIndexed()
    {
      return new VoteContractSet
      {
        DaoId = DaoId,
        VoteContract = VoteContract,
      };
    }
  }

  public partial class HighCouncilEnabled : aelf::IEvent<HighCouncilEnabled>
  {
    public global::System.Collections.Generic.IEnumerable<HighCouncilEnabled> GetIndexed()
    {
      return new List<HighCouncilEnabled>
      {
      };
    }

    public HighCouncilEnabled GetNonIndexed()
    {
      return new HighCouncilEnabled
      {
        DaoId = DaoId,
        HighCouncilConfig = HighCouncilConfig,
      };
    }
  }

  public partial class HighCouncilDisabled : aelf::IEvent<HighCouncilDisabled>
  {
    public global::System.Collections.Generic.IEnumerable<HighCouncilDisabled> GetIndexed()
    {
      return new List<HighCouncilDisabled>
      {
      };
    }

    public HighCouncilDisabled GetNonIndexed()
    {
      return new HighCouncilDisabled
      {
        DaoId = DaoId,
      };
    }
  }

  public partial class SetHighCouncilExecutionSet : aelf::IEvent<SetHighCouncilExecutionSet>
  {
    public global::System.Collections.Generic.IEnumerable<SetHighCouncilExecutionSet> GetIndexed()
    {
      return new List<SetHighCouncilExecutionSet>
      {
      };
    }

    public SetHighCouncilExecutionSet GetNonIndexed()
    {
      return new SetHighCouncilExecutionSet
      {
        DaoId = DaoId,
        HighCouncilExecutionConfig = HighCouncilExecutionConfig,
      };
    }
  }

  public partial class HighCouncilMemberUpdated : aelf::IEvent<HighCouncilMemberUpdated>
  {
    public global::System.Collections.Generic.IEnumerable<HighCouncilMemberUpdated> GetIndexed()
    {
      return new List<HighCouncilMemberUpdated>
      {
      };
    }

    public HighCouncilMemberUpdated GetNonIndexed()
    {
      return new HighCouncilMemberUpdated
      {
        DaoId = DaoId,
        PreviousHighCouncilInfo = PreviousHighCouncilInfo,
        UpdatedHighCouncilInfo = UpdatedHighCouncilInfo,
      };
    }
  }

  public partial class FileInfosUploaded : aelf::IEvent<FileInfosUploaded>
  {
    public global::System.Collections.Generic.IEnumerable<FileInfosUploaded> GetIndexed()
    {
      return new List<FileInfosUploaded>
      {
      };
    }

    public FileInfosUploaded GetNonIndexed()
    {
      return new FileInfosUploaded
      {
        DaoId = DaoId,
        UploadedFiles = UploadedFiles,
      };
    }
  }

  public partial class FileInfosRemoved : aelf::IEvent<FileInfosRemoved>
  {
    public global::System.Collections.Generic.IEnumerable<FileInfosRemoved> GetIndexed()
    {
      return new List<FileInfosRemoved>
      {
      };
    }

    public FileInfosRemoved GetNonIndexed()
    {
      return new FileInfosRemoved
      {
        DaoId = DaoId,
        RemovedFiles = RemovedFiles,
      };
    }
  }

  public partial class PermissionsSet : aelf::IEvent<PermissionsSet>
  {
    public global::System.Collections.Generic.IEnumerable<PermissionsSet> GetIndexed()
    {
      return new List<PermissionsSet>
      {
      };
    }

    public PermissionsSet GetNonIndexed()
    {
      return new PermissionsSet
      {
        DaoId = DaoId,
        Here = Here,
        PermissionInfoList = PermissionInfoList,
      };
    }
  }

  #endregion
  public static partial class DAOContractContainer
  {
    static readonly string __ServiceName = "DAOContract";

    #region Marshallers
    static readonly aelf::Marshaller<global::TomorrowDAO.Contracts.DAO.InitializeInput> __Marshaller_InitializeInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::TomorrowDAO.Contracts.DAO.InitializeInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.Empty.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::TomorrowDAO.Contracts.DAO.CreateDAOInput> __Marshaller_CreateDAOInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::TomorrowDAO.Contracts.DAO.CreateDAOInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::TomorrowDAO.Contracts.DAO.UploadFileInfosInput> __Marshaller_UploadFileInfosInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::TomorrowDAO.Contracts.DAO.UploadFileInfosInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::TomorrowDAO.Contracts.DAO.RemoveFileInfosInput> __Marshaller_RemoveFileInfosInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::TomorrowDAO.Contracts.DAO.RemoveFileInfosInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::TomorrowDAO.Contracts.DAO.GetFileInfoInput> __Marshaller_GetFileInfoInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::TomorrowDAO.Contracts.DAO.GetFileInfoInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::TomorrowDAO.Contracts.DAO.FileInfo> __Marshaller_FileInfo = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::TomorrowDAO.Contracts.DAO.FileInfo.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::TomorrowDAO.Contracts.DAO.SetPermissionsInput> __Marshaller_SetPermissionsInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::TomorrowDAO.Contracts.DAO.SetPermissionsInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::TomorrowDAO.Contracts.DAO.HasPermissionInput> __Marshaller_HasPermissionInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::TomorrowDAO.Contracts.DAO.HasPermissionInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Google.Protobuf.WellKnownTypes.BoolValue> __Marshaller_google_protobuf_BoolValue = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.BoolValue.Parser.ParseFrom);
    #endregion

    #region Methods
    static readonly aelf::Method<global::TomorrowDAO.Contracts.DAO.InitializeInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_Initialize = new aelf::Method<global::TomorrowDAO.Contracts.DAO.InitializeInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "Initialize",
        __Marshaller_InitializeInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::TomorrowDAO.Contracts.DAO.CreateDAOInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_CreateDAO = new aelf::Method<global::TomorrowDAO.Contracts.DAO.CreateDAOInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "CreateDAO",
        __Marshaller_CreateDAOInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::TomorrowDAO.Contracts.DAO.UploadFileInfosInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_UploadFileInfos = new aelf::Method<global::TomorrowDAO.Contracts.DAO.UploadFileInfosInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "UploadFileInfos",
        __Marshaller_UploadFileInfosInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::TomorrowDAO.Contracts.DAO.RemoveFileInfosInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_RemoveFileInfos = new aelf::Method<global::TomorrowDAO.Contracts.DAO.RemoveFileInfosInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "RemoveFileInfos",
        __Marshaller_RemoveFileInfosInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::TomorrowDAO.Contracts.DAO.GetFileInfoInput, global::TomorrowDAO.Contracts.DAO.FileInfo> __Method_GetFileInfo = new aelf::Method<global::TomorrowDAO.Contracts.DAO.GetFileInfoInput, global::TomorrowDAO.Contracts.DAO.FileInfo>(
        aelf::MethodType.View,
        __ServiceName,
        "GetFileInfo",
        __Marshaller_GetFileInfoInput,
        __Marshaller_FileInfo);

    static readonly aelf::Method<global::TomorrowDAO.Contracts.DAO.SetPermissionsInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_SetPermissions = new aelf::Method<global::TomorrowDAO.Contracts.DAO.SetPermissionsInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "SetPermissions",
        __Marshaller_SetPermissionsInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::TomorrowDAO.Contracts.DAO.HasPermissionInput, global::Google.Protobuf.WellKnownTypes.BoolValue> __Method_HasPermission = new aelf::Method<global::TomorrowDAO.Contracts.DAO.HasPermissionInput, global::Google.Protobuf.WellKnownTypes.BoolValue>(
        aelf::MethodType.View,
        __ServiceName,
        "HasPermission",
        __Marshaller_HasPermissionInput,
        __Marshaller_google_protobuf_BoolValue);

    #endregion

    #region Descriptors
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::TomorrowDAO.Contracts.DAO.DaoContractReflection.Descriptor.Services[0]; }
    }

    public static global::System.Collections.Generic.IReadOnlyList<global::Google.Protobuf.Reflection.ServiceDescriptor> Descriptors
    {
      get
      {
        return new global::System.Collections.Generic.List<global::Google.Protobuf.Reflection.ServiceDescriptor>()
        {
          global::AElf.Standards.ACS12.Acs12Reflection.Descriptor.Services[0],
          global::TomorrowDAO.Contracts.DAO.DaoContractReflection.Descriptor.Services[0],
        };
      }
    }
    #endregion

    /// <summary>Base class for the contract of DAOContract</summary>
    // public abstract partial class DAOContractBase : AElf.Sdk.CSharp.CSharpSmartContract<TomorrowDAO.Contracts.DAO.DAOContractState>
    // {
    //   public virtual global::Google.Protobuf.WellKnownTypes.Empty Initialize(global::TomorrowDAO.Contracts.DAO.InitializeInput input)
    //   {
    //     throw new global::System.NotImplementedException();
    //   }
    //
    //   public virtual global::Google.Protobuf.WellKnownTypes.Empty CreateDAO(global::TomorrowDAO.Contracts.DAO.CreateDAOInput input)
    //   {
    //     throw new global::System.NotImplementedException();
    //   }
    //
    //   public virtual global::Google.Protobuf.WellKnownTypes.Empty UploadFileInfos(global::TomorrowDAO.Contracts.DAO.UploadFileInfosInput input)
    //   {
    //     throw new global::System.NotImplementedException();
    //   }
    //
    //   public virtual global::Google.Protobuf.WellKnownTypes.Empty RemoveFileInfos(global::TomorrowDAO.Contracts.DAO.RemoveFileInfosInput input)
    //   {
    //     throw new global::System.NotImplementedException();
    //   }
    //
    //   public virtual global::TomorrowDAO.Contracts.DAO.FileInfo GetFileInfo(global::TomorrowDAO.Contracts.DAO.GetFileInfoInput input)
    //   {
    //     throw new global::System.NotImplementedException();
    //   }
    //
    //   public virtual global::Google.Protobuf.WellKnownTypes.Empty SetPermissions(global::TomorrowDAO.Contracts.DAO.SetPermissionsInput input)
    //   {
    //     throw new global::System.NotImplementedException();
    //   }
    //
    //   public virtual global::Google.Protobuf.WellKnownTypes.BoolValue HasPermission(global::TomorrowDAO.Contracts.DAO.HasPermissionInput input)
    //   {
    //     throw new global::System.NotImplementedException();
    //   }
    //
    // }

    // public static aelf::ServerServiceDefinition BindService(DAOContractBase serviceImpl)
    // {
    //   return aelf::ServerServiceDefinition.CreateBuilder()
    //       .AddDescriptors(Descriptors)
    //       .AddMethod(__Method_Initialize, serviceImpl.Initialize)
    //       .AddMethod(__Method_CreateDAO, serviceImpl.CreateDAO)
    //       .AddMethod(__Method_UploadFileInfos, serviceImpl.UploadFileInfos)
    //       .AddMethod(__Method_RemoveFileInfos, serviceImpl.RemoveFileInfos)
    //       .AddMethod(__Method_GetFileInfo, serviceImpl.GetFileInfo)
    //       .AddMethod(__Method_SetPermissions, serviceImpl.SetPermissions)
    //       .AddMethod(__Method_HasPermission, serviceImpl.HasPermission).Build();
    // }

  }
}
#endregion

