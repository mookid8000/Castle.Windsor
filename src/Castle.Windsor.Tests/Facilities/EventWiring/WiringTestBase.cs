// Copyright 2004-2009 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#if (!SILVERLIGHT)
namespace Castle.Windsor.Tests.Facilities.EventWiring
{
	using Castle.Windsor;
	using Castle.Windsor.Tests;
	using Castle.Windsor.Tests.Facilities.EventWiring.Model;

	using NUnit.Framework;

	[TestFixture]
	public abstract class WiringTestBase
	{
		protected WindsorContainer container;

		[SetUp]
		public void Init()
		{
			container = new WindsorContainer(ConfigHelper.ResolveConfigPath("Facilities/EventWiring/" + GetConfigFile()));
		}

		protected abstract string GetConfigFile();

		[Test]
		public void TriggerSimple()
		{
			var publisher = container.Resolve<SimplePublisher>("SimplePublisher");
			var listener = container.Resolve<SimpleListener>("SimpleListener");

			Assert.IsFalse(listener.Listened);
			Assert.IsNull(listener.Sender);

			publisher.Trigger();

			Assert.IsTrue(listener.Listened);
			Assert.AreSame(publisher, listener.Sender);
		}

		[Test]
		public void TriggerStaticEvent()
		{
			var publisher = container.Resolve<SimplePublisher>("SimplePublisher");
			var listener = container.Resolve<SimpleListener>("SimpleListener2");

			Assert.IsFalse(listener.Listened);
			Assert.IsNull(listener.Sender);

			publisher.StaticTrigger();

			Assert.IsTrue(listener.Listened);
			Assert.AreSame(publisher, listener.Sender);
		}
	}
}

#endif
