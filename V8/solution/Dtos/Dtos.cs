using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace tooorangey.DictionaryItemKeySearch.Dtos
{
    [TableName(TableName)]
    [PrimaryKey("pk")]
    [ExplicitColumns]
    public class DictionaryDto
    {
        public const string TableName = Umbraco.Core.Constants.DatabaseSchema.Tables.DictionaryEntry;
        [Column("pk")]
        [PrimaryKeyColumn]
        public int PrimaryKey { get; set; }

        [Column("id")]
        [Index(IndexTypes.UniqueNonClustered)]
        public Guid UniqueId { get; set; }

        [Column("parent")]
        [NullSetting(NullSetting = NullSettings.Null)]
        [ForeignKey(typeof(DictionaryDto), Column = "id")]
        [Index(IndexTypes.NonClustered, Name = "IX_" + TableName + "_Parent")]
        public Guid? Parent { get; set; }

        [Column("key")]
        [Length(450)]
        [Index(IndexTypes.NonClustered, Name = "IX_cmsDictionary_key")]
        public string Key { get; set; }

        [ResultColumn]
        [Reference(ReferenceType.Many, ColumnName = "UniqueId", ReferenceMemberName = "UniqueId")]
        [Column]
        public List<LanguageTextDto> LanguageTextDtos { get; set; }
    }
    [TableName(Umbraco.Core.Constants.DatabaseSchema.Tables.DictionaryValue)]
    [PrimaryKey("pk")]
    [ExplicitColumns]
    public class LanguageTextDto
    {
        [Column("pk")]
        [PrimaryKeyColumn]
        public int PrimaryKey { get; set; }

        [Column("languageId")]
        [ForeignKey(typeof(LanguageDto), Column = "id")]
        public int LanguageId { get; set; }

        [Column("UniqueId")]
        [ForeignKey(typeof(DictionaryDto), Column = "id")]
        public Guid UniqueId { get; set; }

        [Column("value")]
        [Length(1000)]
        public string Value { get; set; }

        [ResultColumn]
        public string LanguageIsoCode { get; set; }

    }
    [TableName(TableName)]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class LanguageDto
    {
        public const string TableName = Umbraco.Core.Constants.DatabaseSchema.Tables.Language;

        /// <summary>
        /// Gets or sets the identifier of the language.
        /// </summary>
        [Column("id")]
        [PrimaryKeyColumn(IdentitySeed = 2)]
        public short Id { get; set; }

        /// <summary>
        /// Gets or sets the ISO code of the language.
        /// </summary>
        [Column("languageISOCode")]
        [Index(IndexTypes.UniqueNonClustered)]
        [NullSetting(NullSetting = NullSettings.Null)]
        [Length(14)]
        public string IsoCode { get; set; }

        /// <summary>
        /// Gets or sets the culture name of the language.
        /// </summary>
        [Column("languageCultureName")]
        [NullSetting(NullSetting = NullSettings.Null)]
        [Length(100)]
        public string CultureName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the language is the default language.
        /// </summary>
        [Column("isDefaultVariantLang")]
        [Constraint(Default = "0")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the language is mandatory.
        /// </summary>
        [Column("mandatory")]
        [Constraint(Default = "0")]
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Gets or sets the identifier of a fallback language.
        /// </summary>
        [Column("fallbackLanguageId")]
        [ForeignKey(typeof(LanguageDto), Column = "id")]
        [Index(IndexTypes.NonClustered)]
        [NullSetting(NullSetting = NullSettings.Null)]
        public int? FallbackLanguageId { get; set; }
    }

}