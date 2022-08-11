using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class GridDebugValue
    {
        public TextMeshPro Text { get; }
        public List<ITransform> Transforms { get; }

        public GridDebugValue(TextMeshPro text)
        {
            Text = text;
            Transforms = new List<ITransform>();
        }

        public void Add(ITransform transform)
        {
            Transforms.Add(transform);
        }

        public void Remove(ITransform transform)
        {
            Transforms.Remove(transform);
        }

        public override string ToString()
        {
            if(Transforms.Count == 0)
                return string.Empty;

            return string.Join(",\n", Transforms.Select(t => t.Name).ToArray());
        }
    }

    public class GridXZMono : MonoBehaviour
    {
        public IWorldGridXZ<GridDebugValue> Grid { get; private set; }

        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Depth { get; private set; }
        [field: SerializeField] public int CellSize { get; private set; }

        // TODO: o gerenciamento dos transforms será feito nessa classe
        // Posso implementar um dicionário para cada transform e qual o GridCell atual
        // Quando alterar o GridCell atual ele deve ser adicionado no
        // novo GridCell e removido do anterior

        public void Awake()
        {
            Grid = new WorldGridXZ<GridDebugValue>(
                transform.position,
                Width,
                Depth,
                CellSize
            );
        }

        public void TransformToGridPosition(ITransform transform)
        {
            // TODO: implementar um TryUpdateValue
            // TryUpdateValue recebe um callback do valor do GridCell (atual GridPosition)
            // Assim podemos gerenciar o GridCell e utilizar os métodos os Valor,
            // como o Add e Remove
            Grid.TryUpdateValue(
                transform.Position,
                (value) => { value.Add(transform); }
            );
        }
    }
}
