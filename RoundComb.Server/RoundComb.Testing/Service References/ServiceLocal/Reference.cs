﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RoundComb.Testing.ServiceLocal {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RespostaBase", Namespace="http://schemas.datacontract.org/2004/07/CitiusNext.External.Interoperability.Data" +
        "Contracts")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(RoundComb.Testing.ServiceLocal.RespostaMessageExternalIdentificador))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(RoundComb.Testing.ServiceLocal.RespostaSimpleMessage))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(RoundComb.Testing.ServiceLocal.RespostaDocumento))]
    public partial class RespostaBase : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int idErroField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string msgErroField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int idErro {
            get {
                return this.idErroField;
            }
            set {
                if ((this.idErroField.Equals(value) != true)) {
                    this.idErroField = value;
                    this.RaisePropertyChanged("idErro");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string msgErro {
            get {
                return this.msgErroField;
            }
            set {
                if ((object.ReferenceEquals(this.msgErroField, value) != true)) {
                    this.msgErroField = value;
                    this.RaisePropertyChanged("msgErro");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RespostaMessageExternalIdentificador", Namespace="http://schemas.datacontract.org/2004/07/CitiusNext.External.Interoperability.Data" +
        "Contracts")]
    [System.SerializableAttribute()]
    public partial class RespostaMessageExternalIdentificador : RoundComb.Testing.ServiceLocal.RespostaBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private RoundComb.Testing.ServiceLocal.MessageExternalIdentificador entidadeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public RoundComb.Testing.ServiceLocal.MessageExternalIdentificador entidade {
            get {
                return this.entidadeField;
            }
            set {
                if ((object.ReferenceEquals(this.entidadeField, value) != true)) {
                    this.entidadeField = value;
                    this.RaisePropertyChanged("entidade");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RespostaSimpleMessage", Namespace="http://schemas.datacontract.org/2004/07/CitiusNext.External.Interoperability.Data" +
        "Contracts")]
    [System.SerializableAttribute()]
    public partial class RespostaSimpleMessage : RoundComb.Testing.ServiceLocal.RespostaBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private RoundComb.Testing.ServiceLocal.SimpleMessage entidadeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public RoundComb.Testing.ServiceLocal.SimpleMessage entidade {
            get {
                return this.entidadeField;
            }
            set {
                if ((object.ReferenceEquals(this.entidadeField, value) != true)) {
                    this.entidadeField = value;
                    this.RaisePropertyChanged("entidade");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RespostaDocumento", Namespace="http://schemas.datacontract.org/2004/07/CitiusNext.External.Interoperability.Data" +
        "Contracts")]
    [System.SerializableAttribute()]
    public partial class RespostaDocumento : RoundComb.Testing.ServiceLocal.RespostaBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private RoundComb.Testing.ServiceLocal.Documento entidadeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public RoundComb.Testing.ServiceLocal.Documento entidade {
            get {
                return this.entidadeField;
            }
            set {
                if ((object.ReferenceEquals(this.entidadeField, value) != true)) {
                    this.entidadeField = value;
                    this.RaisePropertyChanged("entidade");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Documento", Namespace="http://schemas.datacontract.org/2004/07/CitiusNext.External.Interoperability.Data" +
        "Contracts")]
    [System.SerializableAttribute()]
    public partial class Documento : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] BytesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FilenameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string infoTecnicaField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Bytes {
            get {
                return this.BytesField;
            }
            set {
                if ((object.ReferenceEquals(this.BytesField, value) != true)) {
                    this.BytesField = value;
                    this.RaisePropertyChanged("Bytes");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Filename {
            get {
                return this.FilenameField;
            }
            set {
                if ((object.ReferenceEquals(this.FilenameField, value) != true)) {
                    this.FilenameField = value;
                    this.RaisePropertyChanged("Filename");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string infoTecnica {
            get {
                return this.infoTecnicaField;
            }
            set {
                if ((object.ReferenceEquals(this.infoTecnicaField, value) != true)) {
                    this.infoTecnicaField = value;
                    this.RaisePropertyChanged("infoTecnica");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MessageExternalIdentificador", Namespace="http://schemas.datacontract.org/2004/07/CitiusNext.External.Interoperability.Data" +
        "Contracts")]
    [System.SerializableAttribute()]
    public partial class MessageExternalIdentificador : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int idQueueField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string msgIdField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int idQueue {
            get {
                return this.idQueueField;
            }
            set {
                if ((this.idQueueField.Equals(value) != true)) {
                    this.idQueueField = value;
                    this.RaisePropertyChanged("idQueue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string msgId {
            get {
                return this.msgIdField;
            }
            set {
                if ((object.ReferenceEquals(this.msgIdField, value) != true)) {
                    this.msgIdField = value;
                    this.RaisePropertyChanged("msgId");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SimpleMessage", Namespace="http://schemas.datacontract.org/2004/07/CitiusNext.External.Interoperability.Data" +
        "Contracts")]
    [System.SerializableAttribute()]
    public partial class SimpleMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MessageExternal", Namespace="http://schemas.datacontract.org/2004/07/CitiusNext.External.Interoperability.Data" +
        "Contracts")]
    [System.SerializableAttribute()]
    public partial class MessageExternal : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string msgIdField;
        
        private System.DateTime msgDateField;
        
        private string msgSenderIdField;
        
        private string msgReceiverIdField;
        
        private int msgTypeIdField;
        
        private string msgTypeField;
        
        private string msgExternalRefField;
        
        private string msgBodyField;
        
        private string msgUserField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string msgId {
            get {
                return this.msgIdField;
            }
            set {
                if ((object.ReferenceEquals(this.msgIdField, value) != true)) {
                    this.msgIdField = value;
                    this.RaisePropertyChanged("msgId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=1)]
        public System.DateTime msgDate {
            get {
                return this.msgDateField;
            }
            set {
                if ((this.msgDateField.Equals(value) != true)) {
                    this.msgDateField = value;
                    this.RaisePropertyChanged("msgDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=2)]
        public string msgSenderId {
            get {
                return this.msgSenderIdField;
            }
            set {
                if ((object.ReferenceEquals(this.msgSenderIdField, value) != true)) {
                    this.msgSenderIdField = value;
                    this.RaisePropertyChanged("msgSenderId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=3)]
        public string msgReceiverId {
            get {
                return this.msgReceiverIdField;
            }
            set {
                if ((object.ReferenceEquals(this.msgReceiverIdField, value) != true)) {
                    this.msgReceiverIdField = value;
                    this.RaisePropertyChanged("msgReceiverId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=4)]
        public int msgTypeId {
            get {
                return this.msgTypeIdField;
            }
            set {
                if ((this.msgTypeIdField.Equals(value) != true)) {
                    this.msgTypeIdField = value;
                    this.RaisePropertyChanged("msgTypeId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=5)]
        public string msgType {
            get {
                return this.msgTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.msgTypeField, value) != true)) {
                    this.msgTypeField = value;
                    this.RaisePropertyChanged("msgType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=6)]
        public string msgExternalRef {
            get {
                return this.msgExternalRefField;
            }
            set {
                if ((object.ReferenceEquals(this.msgExternalRefField, value) != true)) {
                    this.msgExternalRefField = value;
                    this.RaisePropertyChanged("msgExternalRef");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=7)]
        public string msgBody {
            get {
                return this.msgBodyField;
            }
            set {
                if ((object.ReferenceEquals(this.msgBodyField, value) != true)) {
                    this.msgBodyField = value;
                    this.RaisePropertyChanged("msgBody");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=8)]
        public string msgUser {
            get {
                return this.msgUserField;
            }
            set {
                if ((object.ReferenceEquals(this.msgUserField, value) != true)) {
                    this.msgUserField = value;
                    this.RaisePropertyChanged("msgUser");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceLocal.ISoapApi")]
    public interface ISoapApi {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISoapApi/getDocumento", ReplyAction="http://tempuri.org/ISoapApi/getDocumentoResponse")]
        RoundComb.Testing.ServiceLocal.RespostaDocumento getDocumento(string idMsgEntity, string fileName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISoapApi/getMessageValidation", ReplyAction="http://tempuri.org/ISoapApi/getMessageValidationResponse")]
        RoundComb.Testing.ServiceLocal.RespostaMessageExternalIdentificador getMessageValidation(string idMsgEntity, RoundComb.Testing.ServiceLocal.MessageExternal msg);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISoapApi/setMessageSubmit", ReplyAction="http://tempuri.org/ISoapApi/setMessageSubmitResponse")]
        RoundComb.Testing.ServiceLocal.RespostaMessageExternalIdentificador setMessageSubmit(string idMsgEntity, RoundComb.Testing.ServiceLocal.MessageExternal msg);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISoapApi/isAlive", ReplyAction="http://tempuri.org/ISoapApi/isAliveResponse")]
        RoundComb.Testing.ServiceLocal.RespostaSimpleMessage isAlive(string idMsgEntity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISoapApi/getMessageCompleteXsd", ReplyAction="http://tempuri.org/ISoapApi/getMessageCompleteXsdResponse")]
        RoundComb.Testing.ServiceLocal.RespostaSimpleMessage getMessageCompleteXsd(string idMsgEntity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISoapApi/getMsgBodyXsd", ReplyAction="http://tempuri.org/ISoapApi/getMsgBodyXsdResponse")]
        RoundComb.Testing.ServiceLocal.RespostaSimpleMessage getMsgBodyXsd(string idMsgEntity, int msgTypeId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISoapApiChannel : RoundComb.Testing.ServiceLocal.ISoapApi, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SoapApiClient : System.ServiceModel.ClientBase<RoundComb.Testing.ServiceLocal.ISoapApi>, RoundComb.Testing.ServiceLocal.ISoapApi {
        
        public SoapApiClient() {
        }
        
        public SoapApiClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SoapApiClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SoapApiClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SoapApiClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public RoundComb.Testing.ServiceLocal.RespostaDocumento getDocumento(string idMsgEntity, string fileName) {
            return base.Channel.getDocumento(idMsgEntity, fileName);
        }
        
        public RoundComb.Testing.ServiceLocal.RespostaMessageExternalIdentificador getMessageValidation(string idMsgEntity, RoundComb.Testing.ServiceLocal.MessageExternal msg) {
            return base.Channel.getMessageValidation(idMsgEntity, msg);
        }
        
        public RoundComb.Testing.ServiceLocal.RespostaMessageExternalIdentificador setMessageSubmit(string idMsgEntity, RoundComb.Testing.ServiceLocal.MessageExternal msg) {
            return base.Channel.setMessageSubmit(idMsgEntity, msg);
        }
        
        public RoundComb.Testing.ServiceLocal.RespostaSimpleMessage isAlive(string idMsgEntity) {
            return base.Channel.isAlive(idMsgEntity);
        }
        
        public RoundComb.Testing.ServiceLocal.RespostaSimpleMessage getMessageCompleteXsd(string idMsgEntity) {
            return base.Channel.getMessageCompleteXsd(idMsgEntity);
        }
        
        public RoundComb.Testing.ServiceLocal.RespostaSimpleMessage getMsgBodyXsd(string idMsgEntity, int msgTypeId) {
            return base.Channel.getMsgBodyXsd(idMsgEntity, msgTypeId);
        }
    }
}