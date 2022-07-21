/**
 * OpenTSPS + Unity3d Extension
 * Created by James George on 11/24/2010
 * 
 * This example is distributed under The MIT License
 *
 * Copyright (c) 2010-2011 James George
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TSPS;

public class csOpenTSPSListener : MonoBehaviour {

	public Dictionary<int,GameObject> peopleObjects = new Dictionary<int,GameObject>();

	
	public void PersonEntered(Person person){

		//Debug.Log(" person entered with ID " + person.id);
		GameObject personObject = new GameObject();
		personObject.name = person.id.ToString();
		personObject.transform.position = new Vector3(0,0,0);
		peopleObjects.Add(person.id,personObject);
	}

	public void PersonUpdated(Person person) {
		if(peopleObjects.ContainsKey(person.id)){
			GameObject cubeToMove = peopleObjects[person.id];
			cubeToMove.transform.position = positionForPerson(person);
			Vector2 tam = new Vector2(person.boundingRectSizeWidth, person.boundingRectSizeHeight);
			cubeToMove.transform.localScale = tam;
		}
	}

	public void PersonWillLeave(Person person){
		//Debug.Log("Person leaving with ID " + person.id);
		if(peopleObjects.ContainsKey(person.id)){
			// Debug.Log("Destroying cube");
			GameObject cubeToRemove = peopleObjects[person.id];
			peopleObjects.Remove(person.id);
			// delete it from the scene	
			Destroy(cubeToRemove);
		}
	}
	
	//maps the OpenTSPS coordinate system into one that matches the size of the boundingPlane
	private Vector3 positionForPerson(Person person){

		return new Vector3( (float)(.5 - person.centroidX) , 0.25f, (float)(person.centroidX - .5));
	}
	
}
