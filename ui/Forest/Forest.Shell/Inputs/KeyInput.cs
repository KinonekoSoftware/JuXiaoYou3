using System.Windows.Input;

namespace Acorisoft.FutureGL.Forest.Inputs
{
    public class KeyInput
    {
        public Action Expression { get; init; }
        public ModifierKeys Modifiers { get; init; }
        public Key Key { get; init; }
    }
}