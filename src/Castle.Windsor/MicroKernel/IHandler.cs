// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
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

namespace Castle.MicroKernel
{
	using System;
	using System.Collections.Generic;

	using Castle.Core;
	using Castle.MicroKernel.Context;

	/// <summary>
	///   Contract for the IHandler, which manages an
	///   component state and coordinates its creation 
	///   and destruction (dispatching to activators, lifestyle managers)
	/// </summary>
	public interface IHandler : ISubDependencyResolver
	{
		/// <summary>
		///   Gets the model of the component being 
		///   managed by this handler.
		/// </summary>
		ComponentModel ComponentModel { get; }

		/// <summary>
		///   Gets the state of the handler
		/// </summary>
		HandlerState CurrentState { get; }

		IEnumerable<Type> Services { get; }

		/// <summary>
		///   Dictionary of String/object used to 
		///   associate data with a component dependency.
		///   For example, if you component SmtpServer depends on 
		///   host and port, you can add those to this
		///   dictionary and the handler will be able to use them.
		/// </summary>
		/// <remarks>
		///   TODO: Document this
		/// </remarks>
		void AddCustomDependencyValue(object key, object value);

		/// <summary>
		///   TODO: Document this
		/// </summary>
		/// <param name = "key"></param>
		/// <returns></returns>
		bool HasCustomParameter(object key);

		/// <summary>
		///   Initializes the handler with a reference to the
		///   kernel.
		/// </summary>
		/// <param name = "kernel"></param>
		void Init(IKernel kernel);

		/// <summary>
		///   Tests whether the handler is already being resolved in given context.
		/// </summary>
		bool IsBeingResolvedInContext(CreationContext context);

		/// <summary>
		///   Implementors should dispose the component instance
		/// </summary>
		/// <param name = "burden"></param>
		/// <returns>true if destroyed.</returns>
		bool Release(Burden burden);

		/// <summary>
		///   TODO: Document this
		/// </summary>
		/// <param name = "key"></param>
		void RemoveCustomDependencyValue(object key);

		/// <summary>
		///   Implementors should return a valid instance 
		///   for the component the handler is responsible.
		///   It should throw an exception in the case the component
		///   can't be created for some reason
		/// </summary>
		/// <returns></returns>
		object Resolve(CreationContext context);

		/// <summary>
		///   Implementors should return a valid instance 
		///   for the component the handler is responsible.
		///   It should return null in the case the component
		///   can't be created for some reason
		/// </summary>
		/// <returns></returns>
		object TryResolve(CreationContext context);

		/// <summary>
		///   Allow to track state changes of a handler that is modified directly.
		///   This can happen if the client calls AddCustomDependencyValue or 
		///   RemoveCustomDependencyValue
		/// </summary>
		event HandlerStateDelegate OnHandlerStateChanged;
	}
}