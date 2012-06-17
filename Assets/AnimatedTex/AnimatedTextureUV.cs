using UnityEngine;

// Source : http://unifycommunity.com/wiki/index.php?title=Animating_Tiled_texture_-_Extended
public class AnimatedTextureUV : MonoBehaviour
{
    //vars for the whole sheet 
	// スプライトシートに配置されたキャラクタの数
	public int colCount =  4;	// 横方向の数
	public int rowCount =  4;	// 縦方向の数
	
	//vars for animation
	public int rowNumber  =  0; // 縦方向のオフセット値
	public int colNumber = 0; 	// 横方向のオフセット値
	public int totalCells = 4;
	public int fps     = 10;
	//Maybe this should be a private var
	private Vector2 offset;
		
	
	//Update
	void Update () 
	{ 
		SetSpriteAnimation( colCount, rowCount, rowNumber, colNumber, totalCells, fps );  
	}
	
	//SetSpriteAnimation
	void SetSpriteAnimation(int colCount ,int rowCount ,int rowNumber ,int colNumber,int totalCells,int fps )
	{
		// An atlas is a single texture containing several smaller textures.
		// It's used for GUI to have not power of two textures and gain space, for example.
		// Here, we have an atlas with 16 faces
	    // Calculate index
	    int index  = (int)(Time.time * fps);
		
	    // Repeat when exhausting all cells
	    index = index % totalCells; // 最後のindexに行った後に0に戻るように「index % 総数」を行う 
									// => 0 1 2 3 / 0 1 2 3 / 0 1 2 3 ...
	    
	    // Size of every cell
		// テクスチャのサイズはUV座標で指定するため 0〜1 の範囲となる。その為、「1.0f / 縦や横方向に並んだキャラクタの数」を行う事で1キャラ分のサイズを求める
	    float sizeX = 1.0f / colCount; // We split the texture in 4 rows and 4 cols
	    float sizeY = 1.0f / rowCount;
	    Vector2 size =  new Vector2(sizeX,sizeY);
	    
	    // split into horizontal and vertical index
	    var uIndex = index % colCount;	// colCountの余りが横(U)方向の座標位置を示す
	    var vIndex = index / colCount;	//  colCountで割ったものが縦(V)方向の座標位置を示す
	 
	    // build offset
	    // v coordinate is the bottom of the image in opengl so we need to invert.
		//  シェーダプログラムで使うオフセット値を計算
	    float offsetX = (uIndex + colNumber) * size.x;
	    float offsetY = (vIndex + rowNumber) * size.y;
	    Vector2 offset = new Vector2(offsetX,offsetY);
	    
		// We give the change to the material
		// This has the same effect as changing the offset value of the material in the editor.
		// シェーダプログラムにデータを渡す
	    renderer.material.SetTextureOffset ("_MainTex", offset); // テクスチャを使用する開始位置をオフセット値として渡す
	    renderer.material.SetTextureScale  ("_MainTex", size); // 使用するテクスチャのサイズを渡す
	}
}