using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Itec.ORM
{
    public class WithOptions {
        internal class WithOptionsReference
        {
            public bool IsMany { get; set; }
            public Expression PropertyExpression { get; set; }


            public WithOptions WithOpts { get; set; }
        }

        public LambdaExpression Primary { get; set; }

        public LambdaExpression Foreign { get; set; }

        public string Fieldnames { get; set; }

        public string Tablename { get; set; }

        public string InterTablename { get; set; }

        
        internal IList<WithOptionsReference> References
        {
            get;set;
        }
    }
    public class WithOptions<TPrimary,TForeign>:WithOptions
    {
        
        public new Expression<Func<TPrimary,object>> Primary {
            get { return base.Primary as Expression<Func<TPrimary, object>>; }
            set { base.Primary = value; }
        }

        public new Expression<Func<TForeign, object>> Foreign {
            get { return base.Foreign as Expression<Func<TForeign, object>>; }
            set { base.Foreign = value; }
        }

        

        

        public WithOptions<TPrimary, TForeign> WithOne<TOther>(Expression<Func<TForeign, TOther>> prop, WithOptions<TForeign, TOther> opts)
        {
            if (References == null) this.References = new List<WithOptionsReference>();
            this.References.Add(new WithOptionsReference() {
                PropertyExpression = prop,
                WithOpts = opts
            });
            return this;
        }

        public WithOptions<TPrimary, TForeign> WithMany<TOther>(Expression<Func<TForeign, IEnumerable<TOther>>> prop, WithOptions<TForeign, TOther> opts)
        {
            if (References == null) this.References = new List<WithOptionsReference>();
            this.References.Add(new WithOptionsReference()
            {
                IsMany = true,
                PropertyExpression = prop,
                WithOpts = opts
            });
            
            return this;
        }
    }
}
