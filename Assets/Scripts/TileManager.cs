using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile interactedTile;
    
    // Dictionary untuk track crop di setiap tile
    private Dictionary<Vector3Int, GameObject> crops = new Dictionary<Vector3Int, GameObject>();

    void Start()
    {
        foreach (var position in interactableMap.cellBounds.allPositionsWithin) 
        {
            TileBase tile = interactableMap.GetTile(position);

            if(tile != null && tile.name == "Interactable_Visible")
            {
                interactableMap.SetTile(position, hiddenInteractableTile);
            }
        }
    }

    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);
        return tile != null && tile == hiddenInteractableTile;
    }

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, interactedTile);
    }

    public Vector3Int GetTilePosition(Vector3 worldPosition)
    {
        return interactableMap.WorldToCell(worldPosition);
    }
    
    public Vector3 GetTileWorldPosition(Vector3Int tilePosition)
    {
        return interactableMap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0f);
    }
    
    public void PlantCrop(Vector3Int position, GameObject crop)
    {
        if (!crops.ContainsKey(position))
        {
            crops.Add(position, crop);
            SetInteracted(position); // Ubah tile jadi "interacted"
        }
    }
    
    public bool HasCrop(Vector3Int position)
    {
        return crops.ContainsKey(position) && crops[position] != null;
    }
    
    public GameObject GetCropAt(Vector3Int position)
    {
        if (crops.ContainsKey(position))
        {
            return crops[position];
        }
        return null;
    }
    
    public void RemoveCrop(Vector3Int position)
    {
        if (crops.ContainsKey(position))
        {
            crops.Remove(position);
        }
    }
}