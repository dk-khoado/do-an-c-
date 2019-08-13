using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class TypeGameModel
{
    public List<TypeGame> data;
}
[Serializable]
public class TypeGame
{
    public int id;
    public string name_bai;
}

