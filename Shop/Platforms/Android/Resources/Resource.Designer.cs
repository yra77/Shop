using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Platforms.Android.Resources
{
    public partial class Resource : Component
    {
        public Resource()
        {
            InitializeComponent();
        }

        public Resource(System.ComponentModel.IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
