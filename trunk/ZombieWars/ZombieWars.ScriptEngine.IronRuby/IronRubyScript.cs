using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;
using ZombieWars.Core.Npc;
using ZombieWars.Core.Maps;

namespace ZombieWars.ScriptEngines.IronRubyScript
{
    public class IronRubyScript : Script
    {
        static ScriptRuntime runtime;
        static Microsoft.Scripting.Hosting.ScriptEngine _rubyEngine = null;
        static ScriptScope _scope = null;
        
        static IronRubyScript()
        {
            runtime = IronRuby.Ruby.CreateRuntime();
            _rubyEngine = runtime.GetEngine("Ruby");
            _scope = _rubyEngine.CreateScope();
        }

        public override INpcAction Execute(NpcState NpcState)
        {
            INpcAction action = null;
            try
            {
                _scope.SetVariable("curTarget", NpcState.Target);
                _scope.SetVariable("map", NpcState.MapState);
                var result = _rubyEngine.Execute(this.Content, _scope);
                _scope.RemoveVariable("curTarget");
                _scope.RemoveVariable("map");
                action = (INpcAction)result;
            }
            catch { }
            finally
            {
            }
            return action;
        }
    }
}
