﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Baza
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="PED")]
	public partial class DataClasses1DataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void Insertrpackage(rpackage instance);
    partial void Updaterpackage(rpackage instance);
    partial void Deleterpackage(rpackage instance);
    #endregion
		
		public DataClasses1DataContext() : 
				base(global::Baza.Properties.Settings.Default.PEDConnectionString2, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Analysi> Analysis
		{
			get
			{
				return this.GetTable<Analysi>();
			}
		}
		
		public System.Data.Linq.Table<rpackage> rpackages
		{
			get
			{
				return this.GetTable<rpackage>();
			}
		}
		
		public System.Data.Linq.Table<CustItemDailyCon> CustItemDailyCons
		{
			get
			{
				return this.GetTable<CustItemDailyCon>();
			}
		}
		
		public System.Data.Linq.Table<ItemConsumption> ItemConsumptions
		{
			get
			{
				return this.GetTable<ItemConsumption>();
			}
		}
		
		public System.Data.Linq.Table<PurchaseHistory> PurchaseHistories
		{
			get
			{
				return this.GetTable<PurchaseHistory>();
			}
		}
		
		public System.Data.Linq.Table<PurchasePeriod> PurchasePeriods
		{
			get
			{
				return this.GetTable<PurchasePeriod>();
			}
		}
		
		public System.Data.Linq.Table<RecommendHistory> RecommendHistories
		{
			get
			{
				return this.GetTable<RecommendHistory>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Analysis")]
	public partial class Analysi
	{
		
		private string _CustNo;
		
		private string _ItemNo;
		
		private int _DataPoints;
		
		private double _AvgPurchasePeriod;
		
		private double _MinPurchasePeriod;
		
		private double _MaxPurchasePeriod;
		
		private double _PurchasePeriodStDev;
		
		private double _NormalizedPurchasePeriodStDev;
		
		public Analysi()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CustNo", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string CustNo
		{
			get
			{
				return this._CustNo;
			}
			set
			{
				if ((this._CustNo != value))
				{
					this._CustNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ItemNo", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ItemNo
		{
			get
			{
				return this._ItemNo;
			}
			set
			{
				if ((this._ItemNo != value))
				{
					this._ItemNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataPoints", DbType="Int NOT NULL")]
		public int DataPoints
		{
			get
			{
				return this._DataPoints;
			}
			set
			{
				if ((this._DataPoints != value))
				{
					this._DataPoints = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AvgPurchasePeriod", DbType="Float NOT NULL")]
		public double AvgPurchasePeriod
		{
			get
			{
				return this._AvgPurchasePeriod;
			}
			set
			{
				if ((this._AvgPurchasePeriod != value))
				{
					this._AvgPurchasePeriod = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MinPurchasePeriod", DbType="Float NOT NULL")]
		public double MinPurchasePeriod
		{
			get
			{
				return this._MinPurchasePeriod;
			}
			set
			{
				if ((this._MinPurchasePeriod != value))
				{
					this._MinPurchasePeriod = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MaxPurchasePeriod", DbType="Float NOT NULL")]
		public double MaxPurchasePeriod
		{
			get
			{
				return this._MaxPurchasePeriod;
			}
			set
			{
				if ((this._MaxPurchasePeriod != value))
				{
					this._MaxPurchasePeriod = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PurchasePeriodStDev", DbType="Float NOT NULL")]
		public double PurchasePeriodStDev
		{
			get
			{
				return this._PurchasePeriodStDev;
			}
			set
			{
				if ((this._PurchasePeriodStDev != value))
				{
					this._PurchasePeriodStDev = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NormalizedPurchasePeriodStDev", DbType="Float NOT NULL")]
		public double NormalizedPurchasePeriodStDev
		{
			get
			{
				return this._NormalizedPurchasePeriodStDev;
			}
			set
			{
				if ((this._NormalizedPurchasePeriodStDev != value))
				{
					this._NormalizedPurchasePeriodStDev = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.rpackages")]
	public partial class rpackage : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Name;
		
		private string _Owner;
		
		private byte _Scope;
		
		private System.Data.Linq.Binary _ZipFile;
		
		private string _Manifest;
		
		private int _Attributes;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnOwnerChanging(string value);
    partial void OnOwnerChanged();
    partial void OnScopeChanging(byte value);
    partial void OnScopeChanged();
    partial void OnZipFileChanging(System.Data.Linq.Binary value);
    partial void OnZipFileChanged();
    partial void OnManifestChanging(string value);
    partial void OnManifestChanged();
    partial void OnAttributesChanging(int value);
    partial void OnAttributesChanged();
    #endregion
		
		public rpackage()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Owner", DbType="NVarChar(128) NOT NULL", CanBeNull=false)]
		public string Owner
		{
			get
			{
				return this._Owner;
			}
			set
			{
				if ((this._Owner != value))
				{
					this.OnOwnerChanging(value);
					this.SendPropertyChanging();
					this._Owner = value;
					this.SendPropertyChanged("Owner");
					this.OnOwnerChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Scope", DbType="TinyInt NOT NULL")]
		public byte Scope
		{
			get
			{
				return this._Scope;
			}
			set
			{
				if ((this._Scope != value))
				{
					this.OnScopeChanging(value);
					this.SendPropertyChanging();
					this._Scope = value;
					this.SendPropertyChanged("Scope");
					this.OnScopeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ZipFile", DbType="VarBinary(MAX) NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public System.Data.Linq.Binary ZipFile
		{
			get
			{
				return this._ZipFile;
			}
			set
			{
				if ((this._ZipFile != value))
				{
					this.OnZipFileChanging(value);
					this.SendPropertyChanging();
					this._ZipFile = value;
					this.SendPropertyChanged("ZipFile");
					this.OnZipFileChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Manifest", DbType="NVarChar(MAX)")]
		public string Manifest
		{
			get
			{
				return this._Manifest;
			}
			set
			{
				if ((this._Manifest != value))
				{
					this.OnManifestChanging(value);
					this.SendPropertyChanging();
					this._Manifest = value;
					this.SendPropertyChanged("Manifest");
					this.OnManifestChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Attributes", DbType="Int NOT NULL")]
		public int Attributes
		{
			get
			{
				return this._Attributes;
			}
			set
			{
				if ((this._Attributes != value))
				{
					this.OnAttributesChanging(value);
					this.SendPropertyChanging();
					this._Attributes = value;
					this.SendPropertyChanged("Attributes");
					this.OnAttributesChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.CustItemDailyCons")]
	public partial class CustItemDailyCon
	{
		
		private string _CustNo;
		
		private string _ItemNo;
		
		private int _ProcessingDate;
		
		private double _AvgConsumption;
		
		public CustItemDailyCon()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CustNo", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string CustNo
		{
			get
			{
				return this._CustNo;
			}
			set
			{
				if ((this._CustNo != value))
				{
					this._CustNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ItemNo", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ItemNo
		{
			get
			{
				return this._ItemNo;
			}
			set
			{
				if ((this._ItemNo != value))
				{
					this._ItemNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProcessingDate", DbType="Int NOT NULL")]
		public int ProcessingDate
		{
			get
			{
				return this._ProcessingDate;
			}
			set
			{
				if ((this._ProcessingDate != value))
				{
					this._ProcessingDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AvgConsumption", DbType="Float NOT NULL")]
		public double AvgConsumption
		{
			get
			{
				return this._AvgConsumption;
			}
			set
			{
				if ((this._AvgConsumption != value))
				{
					this._AvgConsumption = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ItemConsumption")]
	public partial class ItemConsumption
	{
		
		private string _ItemNo;
		
		private int _Date;
		
		private double _Consumption;
		
		public ItemConsumption()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ItemNo", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ItemNo
		{
			get
			{
				return this._ItemNo;
			}
			set
			{
				if ((this._ItemNo != value))
				{
					this._ItemNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="Int NOT NULL")]
		public int Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this._Date = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Consumption", DbType="Float NOT NULL")]
		public double Consumption
		{
			get
			{
				return this._Consumption;
			}
			set
			{
				if ((this._Consumption != value))
				{
					this._Consumption = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.PurchaseHistory")]
	public partial class PurchaseHistory
	{
		
		private string _CustNo;
		
		private string _ItemNo;
		
		private int _InvDate;
		
		private int _InvQty;
		
		public PurchaseHistory()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CustNo", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string CustNo
		{
			get
			{
				return this._CustNo;
			}
			set
			{
				if ((this._CustNo != value))
				{
					this._CustNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ItemNo", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ItemNo
		{
			get
			{
				return this._ItemNo;
			}
			set
			{
				if ((this._ItemNo != value))
				{
					this._ItemNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InvDate", DbType="Int NOT NULL")]
		public int InvDate
		{
			get
			{
				return this._InvDate;
			}
			set
			{
				if ((this._InvDate != value))
				{
					this._InvDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InvQty", DbType="Int NOT NULL")]
		public int InvQty
		{
			get
			{
				return this._InvQty;
			}
			set
			{
				if ((this._InvQty != value))
				{
					this._InvQty = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.PurchasePeriods")]
	public partial class PurchasePeriod
	{
		
		private string _CustNo;
		
		private string _ItemNo;
		
		private System.DateTime _InvDateCurr;
		
		private System.DateTime _InvDatePrior;
		
		private int _PurchasePeriod1;
		
		public PurchasePeriod()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CustNo", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string CustNo
		{
			get
			{
				return this._CustNo;
			}
			set
			{
				if ((this._CustNo != value))
				{
					this._CustNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ItemNo", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ItemNo
		{
			get
			{
				return this._ItemNo;
			}
			set
			{
				if ((this._ItemNo != value))
				{
					this._ItemNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InvDateCurr", DbType="DateTime NOT NULL")]
		public System.DateTime InvDateCurr
		{
			get
			{
				return this._InvDateCurr;
			}
			set
			{
				if ((this._InvDateCurr != value))
				{
					this._InvDateCurr = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InvDatePrior", DbType="DateTime NOT NULL")]
		public System.DateTime InvDatePrior
		{
			get
			{
				return this._InvDatePrior;
			}
			set
			{
				if ((this._InvDatePrior != value))
				{
					this._InvDatePrior = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="PurchasePeriod", Storage="_PurchasePeriod1", DbType="Int NOT NULL")]
		public int PurchasePeriod1
		{
			get
			{
				return this._PurchasePeriod1;
			}
			set
			{
				if ((this._PurchasePeriod1 != value))
				{
					this._PurchasePeriod1 = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.RecommendHistory")]
	public partial class RecommendHistory
	{
		
		private string _CustNo;
		
		private string _ItemNo;
		
		private int _ProcessingDate;
		
		public RecommendHistory()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CustNo", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string CustNo
		{
			get
			{
				return this._CustNo;
			}
			set
			{
				if ((this._CustNo != value))
				{
					this._CustNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ItemNo", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ItemNo
		{
			get
			{
				return this._ItemNo;
			}
			set
			{
				if ((this._ItemNo != value))
				{
					this._ItemNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProcessingDate", DbType="Int NOT NULL")]
		public int ProcessingDate
		{
			get
			{
				return this._ProcessingDate;
			}
			set
			{
				if ((this._ProcessingDate != value))
				{
					this._ProcessingDate = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
