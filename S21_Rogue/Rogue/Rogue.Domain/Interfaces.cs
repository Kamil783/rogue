using Rogue.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain
{
    public interface IRender
    {
        public void RenderGame(GameSession session);
        public void Initialize(GameEngine engine);
    }

    public interface IInput
    {
        public PlayerCommand ReadInput();
    }
}