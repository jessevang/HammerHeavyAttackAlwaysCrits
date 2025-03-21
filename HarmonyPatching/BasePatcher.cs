//Credit and Source for harming patching is from https://github.com/spacechase0/StardewValleyMods

using System;
using System.Reflection;
using HarmonyLib;
using StardewModdingAPI;

namespace Spacechase.Shared.Patching
{
    /// <summary>Provides base implementation logic for <see cref="IPatcher"/> instances.</summary>
    internal abstract class BasePatcher : IPatcher
    {
        /*********
        ** Public methods
        *********/
        /// <inheritdoc />
        public abstract void Apply(Harmony harmony, IMonitor monitor);


        /*********
        ** Protected methods
        *********/
        /// <summary>Get a method and assert that it was found.</summary>
        /// <typeparam name="TTarget">The type containing the method.</typeparam>
        /// <param name="parameters">The method parameter types, or <c>null</c> if it's not overloaded.</param>
        protected ConstructorInfo RequireConstructor<TTarget>(params Type[] parameters)
        {
            return PatchHelper.RequireConstructor<TTarget>(parameters);
        }

        /// <summary>Get a method and assert that it was found.</summary>
        /// <typeparam name="TTarget">The type containing the method.</typeparam>
        /// <param name="name">The method name.</param>
        /// <param name="parameters">The method parameter types, or <c>null</c> if it's not overloaded.</param>
        /// <param name="generics">The method generic types, or <c>null</c> if it's not generic.</param>
        protected MethodInfo RequireMethod<TTarget>(string name, Type[] parameters = null, Type[] generics = null, HarmonyMethod prefix = null)
        {
            return PatchHelper.RequireMethod<TTarget>(name, parameters, generics);
        }

        /// <summary>Get a Harmony patch method on the current patcher instance.</summary>
        /// <param name="name">The method name.</param>
        /// <param name="priority">The patch priority to apply, usually specified using Harmony's <see cref="Priority"/> enum, or <c>null</c> to keep the default value.</param>
        /// <param name="before">The Harmony patch ID before which this patch should be applied, if any.</param>
        protected HarmonyMethod GetHarmonyMethod(string name, int? priority = null, string before = null, string after = null)
        {
            var method = new HarmonyMethod(
                AccessTools.Method(this.GetType(), name)
                ?? throw new InvalidOperationException($"Can't find patcher method {PatchHelper.GetMethodString(this.GetType(), name)}.")
            );

            if (priority.HasValue)
                method.priority = priority.Value;

            if (before != null)
                method.before = new[] { before };

            if (after != null)
                method.after = new[] { after };

            return method;
        }
    }
}