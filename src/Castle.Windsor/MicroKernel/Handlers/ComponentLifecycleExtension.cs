﻿// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
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

namespace Castle.MicroKernel.Handlers
{
	using System.Collections.Generic;

	public class ComponentLifecycleExtension : IResolveExtension
	{
		private readonly List<ComponentResolvingDelegate> resolvers = new List<ComponentResolvingDelegate>(4);
		private IKernel kernel;

		public void AddHandler(ComponentResolvingDelegate handler)
		{
			resolvers.Add(handler);
		}

		public void Init(IKernel kernel, IHandler handler)
		{
			this.kernel = kernel;
		}

		public void Intercept(ResolveInvocation invocation)
		{
			List<ComponentReleasingDelegate> releasers = null;
			if (resolvers.Count > 0)
			{
				foreach (var resolver in resolvers)
				{
					var releaser = resolver(kernel, invocation.Context);
					if (releaser != null)
					{
						if (releasers == null)
						{
							releasers = new List<ComponentReleasingDelegate>();
							invocation.RequireDecommission();
						}
						releasers.Add(releaser);
					}
				}
			}

			invocation.Proceed();

			if (releasers == null)
			{
				return;
			}
			var burden = invocation.Burden;
			if (burden == null)
			{
				return;
			}
			burden.Released += delegate { releasers.ForEach(r => r.Invoke(kernel)); };
		}
	}
}