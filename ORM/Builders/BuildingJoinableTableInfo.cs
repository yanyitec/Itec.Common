using Itec.Metas;
using Itec.Queriables;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Itec.ORM.Builders
{
    public class BuildingJoinableTableInfo:BuildingTableInfo
    {
        public BuildingJoinableTableInfo(IMetaClass cls) {
            this.MetaClass = cls;
        }
        public IMetaClass MetaClass { get;private set; }

        public BuildingReferenceInfo AppendReference(Expression propExpr,  WithOptions withOpts = null)
        {

            var memberName = (propExpr as MemberExpression).Member.Name;
            var referenceProp = this.MetaClass[memberName];
            var joinedClass = MetaFactory.Default.GetClass(referenceProp.PropertyType);

            var referenceTable = new BuildingJoinableTableInfo(joinedClass);
            referenceTable.Tablename = withOpts?.Tablename ?? Builder.GetTablename(joinedClass);
            referenceTable.Fieldnames = withOpts?.Fieldnames;

            if (withOpts != null && withOpts.References!=null) {
                foreach (var join in withOpts.References) {
                    var refc = referenceTable.AppendReference(join.PropertyExpression, join.WithOpts);
                    if (join.IsMany) refc.ReferenceType = refc.InterTable == null ? ReferenceTypes.WithMany : ReferenceTypes.ManyMany;
                }
            }

            var reference = new BuildingReferenceInfo()
            {

                PrimaryKey = (withOpts?.Primary?.Body as MemberExpression)?.Member?.Name ?? "Id",
                ForeignKey = (withOpts?.Foreign?.Body as MemberExpression)?.Member?.Name ?? this.Tablename + "Id",
                ReferenceProperty = referenceProp,
                ReferenceTableInfo = referenceTable
            };
            
            this.References.Add(memberName,reference);
            return reference;

        }

        public BuildingJoinableTableInfo WithOne<TPrimary,TForeign>(Expression<Func<TPrimary, TForeign>> prop, WithOptions<TPrimary, TForeign> opts)
        {
            var reference = this.AppendReference(prop, opts);
            reference.ReferenceType = ReferenceTypes.WithOne;
            //this.RootTable.References.Add(reference.ReferenceProperty.Name, reference);
            return this;
        }

        public BuildingJoinableTableInfo WithMany<TPrimary,TForeign>(Expression<Func<TPrimary, IEnumerable<TForeign>>> prop, WithOptions<TPrimary, TForeign> opts)
        {
            var reference = this.AppendReference(prop, opts);
            if (opts != null)
            {
                if (opts.InterTablename != null) reference.ReferenceType = ReferenceTypes.ManyMany;
                else reference.ReferenceType = ReferenceTypes.WithMany;
            }

            
            return this;
        }

    }
}
