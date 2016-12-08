using System.Collections.Generic;
using MiddleMan;
using XoGame.Models;

namespace XoGame.Events
{
    public class TopTenChanged : MyEvent<List<TopScore>>
    {
    }
}