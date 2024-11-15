using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
   [SerializeField] GameObject _tileModel;
   [SerializeField] private float _tileSpawnOffset = 15f;
   [SerializeField] private float _lowerSpeed = 1f;
   [SerializeField] private float _lowerSpeedDiff = 10f;
   [SerializeField] private GameObject _spawnPlane;
   
   [SerializeField] private Transform _tileUnitPlacement;
   public Transform TileUnitPlacement => _tileUnitPlacement;

   [SerializeField] private Unit _unitToSpawn;
   private Vector3 _startPosition;

   public Action OnTileLowered;
   private Vector3 _initialScale;
   private Vector2 _coords;
   [SerializeField]private GameObject _spawnIndicator;
   
   private Unit _occuipiedUnit;
   public Unit OccuipiedUnit => _occuipiedUnit;
   private void OnValidate()
   {
      _spawnIndicator.gameObject.SetActive(_unitToSpawn != null);
   }

   private void Awake()
   {
      _spawnIndicator.gameObject.SetActive(false);
   }

   public void ActivateTile()
   {
      _initialScale = _tileModel.transform.localScale;
      StartCoroutine(LowerTileForSpawn());
   }

   private IEnumerator LowerTileForSpawn()
   {
      var spendDiff = Random.Range(-_lowerSpeedDiff, _lowerSpeedDiff);
      var distanceLowered = 0f;
      var lowerSpeed = _lowerSpeed + spendDiff;
      while (distanceLowered < _tileSpawnOffset)
      {
         var amountToLower = Time.deltaTime * lowerSpeed;
         _tileModel.transform.position += Vector3.down * amountToLower;
         distanceLowered += amountToLower;
         yield return null;
      }

      _tileModel.transform.position = _startPosition;
      OnTileLowered.Invoke();
   }

   public void InitializeModel()
   {
      _startPosition = _tileModel.transform.position;
      _tileModel.SetActive(true);
      _tileModel.transform.position = new Vector3(_tileModel.transform.position.x,
         _tileModel.transform.position.y + _tileSpawnOffset,
         _tileModel.transform.position.z);
   }

   public void SpawnUnit()
   {
     if (_unitToSpawn == null) return;
      StartCoroutine(SpawnUnitWithPortal());
   }

   private IEnumerator SpawnUnitWithPortal()
   {
      
      ////////// Lerp Color from clear to black
         
         float counter = 0;
         float duration = .1f;
         var spawnPlaneRenderer = _spawnPlane.GetComponent<Renderer>();
         while (counter < duration)
         {
            counter += Time.deltaTime;

            float colorTime = counter / duration;
            Debug.Log (colorTime);

            //Change color
            spawnPlaneRenderer.material.color = Color.Lerp (Color.clear, Color.black, counter / duration);
            //Wait for a frame
            yield return null;
         }
      
   
         ////////// X and Y scales to grow dot into square
        
   float xScalecounter = 0;
   float xScaleduration = 1f;
   
   float yScalecounter = 0;
   float yScaleduration =1f;
   
   
   _spawnPlane.transform.DOScaleX(.2f, xScaleduration);
  
  
      while (xScalecounter < xScaleduration)
      {
         xScalecounter += Time.deltaTime;
         yield return null;
      }
      
      _spawnPlane.transform.DOScaleZ(.2f, yScaleduration);
      
      while (yScalecounter < yScaleduration)
      {
         yScalecounter += Time.deltaTime;
         
         yield return null;
      }
    
   var spawnedUnit = Instantiate(_unitToSpawn, transform.position, Quaternion.identity);
   SetToOccupyTile(spawnedUnit);
   yield return new WaitForSeconds(3f);
  
   
   float counterTwo = 0;
   float durationTwo = 1f;
   Color goalColor = new Color(217, 217, 217, 1);
   while (counterTwo < durationTwo)
   {
      counterTwo += Time.deltaTime;
      
      //Change color
      
      spawnPlaneRenderer.material.color = Color.Lerp (Color.black, Color.white , counterTwo / durationTwo);
      //Wait for a frame
      yield return null;
   }

   spawnPlaneRenderer.material.color = goalColor; 
   _spawnPlane.gameObject.SetActive(false);
      yield return null;
   }

   public void SetCoords(int x, int y)
   {
      _coords.x =x;
      _coords.y =y;
   }

   public void SetToOccupyTile(Unit selectedUnit)
   {
      _occuipiedUnit = selectedUnit;
      selectedUnit.MoveToTile(this);
   }

   public void ClearOccupiedUnit()
   {
      _occuipiedUnit = null;
   }
}
