﻿//-------------------------------------------------------------------------------
// <copyright file="BindingExtensionsTest.cs" company="bbv Software Services AG">
//   Copyright (c) 2012
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace StockTicker.Extensions
{
    using System;

    using Caliburn.Micro;

    using Moq;

    using Ninject;

    using Xunit;

    public class BindingExtensionsTest : IDisposable
    {
        private readonly Mock<IEventAggregator> eventAggregator;

        private readonly StandardKernel kernel;

        public BindingExtensionsTest()
        {
            this.eventAggregator = new Mock<IEventAggregator>();
            
            this.kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false });
            this.kernel.Bind<IEventAggregator>().ToConstant(this.eventAggregator.Object);
        }

        [Fact]
        public void ShouldSubscribeAndUnsubscribe()
        {
            this.kernel.Bind<object>().ToSelf().RegisterOnEventAggregator();

            using (var block = this.kernel.BeginBlock())
            {
                block.Get<object>();
            }

            this.eventAggregator.Verify(e => e.Subscribe(It.IsAny<object>()));
            this.eventAggregator.Verify(e => e.Unsubscribe(It.IsAny<object>()));
        }

        public void Dispose()
        {
            this.kernel.Dispose();
        }
    }
}