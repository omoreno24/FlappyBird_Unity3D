#pragma strict
//******DECLARATION*************************
 var action:action[];
 var timer:float;
 private var count:int=0;
 private var nextFire:float=0.0f;
 private var FireRate:float=0.3f;
 private var actionTime:boolean=false;
 private var i:int=0;
 private var Enemy:enemy[];
 private var currentEnemy:enemy;
 private var InstCant:int;
 private var TotalAct:int;
 private var TotalEnemies:int;
 private var randomPos:Vector3;
 private var randomRot:Quaternion;
 public var maxInstancePos:Vector3;
 public var minInstancePos:Vector3;
//*****************************************
function Start () {
	Ready();
	maxInstancePos=Vector3(Screen.width,Screen.height,transform.position.x);
	maxInstancePos=Camera.main.ScreenToWorldPoint(maxInstancePos);
	minInstancePos=Vector3(0,0,transform.position.x);
	minInstancePos=Camera.main.ScreenToWorldPoint(minInstancePos);
	TotalAct=action.Length;
}

//*******UPDATE****************************************************
function Update () 
{
	timer+=1*Time.deltaTime;
	
	if(actionTime==true && action[count].isCompleted==false)
	{
		if(InstCant>0)
		{
			if(Time.time>nextFire)
			{
				nextFire=Time.time+FireRate;
				if(currentEnemy.RandomPos==true)
				{
					randomPos=getRandomPos();
					Instantiate(currentEnemy.enemy,randomPos,currentEnemy.transform.rotation);
					InstCant-=1;
				}
				else
				{
					Instantiate(currentEnemy.enemy,currentEnemy.transform.position,currentEnemy.transform.rotation);
					InstCant-=1;
				}
			}
		}
		else
		{
			i++;
			
			if(i<TotalEnemies)
			{
				currentEnemy=Enemy[i];
				InstCant=currentEnemy.cantidad;
				FireRate=currentEnemy.instanceRate;
			}
			else
			{
				i=0;
				action[count].isCompleted=true;
				count++;
				if(count==TotalAct)
				{
					count=TotalAct-1;
				}
				Ready();
				actionTime=false;
			}
		}	
	}
	
	if(count<TotalAct)
	{
		if(timer>=action[count].time)
		{
			actionTime=true;
		}
		if(count+1<TotalAct)
		{	
			if(timer>=action[count+1].time && action[count+1].isCompleted==false )
			{
				action[count].isCompleted=true;	
				count++;
				i=0;
				Ready();
			}
		}
	}
	else
	{
		actionTime=false;
	}
}

//********FUNCION READY*********************************
//esta funcion se encarga de actualizar todas las varia-
//-bles al siguientes tiempo de Accion.
function Ready()
{
	Enemy=action[count].enemy;
	TotalEnemies=Enemy.Length;
	currentEnemy=Enemy[i];
	InstCant=currentEnemy.cantidad;
	FireRate=currentEnemy.instanceRate;
	
}
function getRandomPos()
{
	var CamPoint:Vector3=new Vector3(Screen.width-10,Random.Range(0,Screen.height),Camera.main.WorldToScreenPoint(transform.position).z);
	var InstancePos:Vector3=Camera.main.ScreenToWorldPoint(CamPoint);
	return InstancePos;
}
//*********CLASE ACTION*********************************
//Esta clase contiene toda la informacion de las accion-
//-es a ejecutar.
class action
{
	var isCompleted:boolean=false;//true si la accion eta completa
	var time:float;//tiempo en el que se ejecutara la accion
	var enemy:enemy[];//los enemigos que saldran en esta accion
}

//********CLASE ENEMIGO**********************************
//contiene la informacion de los enemigos a instancear.
class enemy
{
	var cantidad:int;//cantidad de enemigos a instancear
	var instanceRate:float;//velocidad de instanciado
	var enemy:GameObject;//enemigo a instancear
	var transform:Transform;//info para el instanceado
	var RandomPos:boolean;//si se desea posicionar aleatoriamente.
}