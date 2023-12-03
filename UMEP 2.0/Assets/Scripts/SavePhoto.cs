using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Events;

public class SavePhoto : MonoBehaviour
{
    public string FinalPath;
    public UnityEvent Function_onPickedFile_Return;
    public UnityEvent Function_onSaved_Return;

    public NativeGallery.Permission permissionGal;

	public void PickPhotoGallery()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                FinalPath = path;
                Variables.ActiveScene.Set("Picked File", FinalPath);
                Function_onPickedFile_Return.Invoke();

            }
        });
    }
	
    public void SavePhotoToCameraRoll(Texture2D MyTexture, string AlbumName, string filename)
    {
        NativeGallery.SaveImageToGallery(MyTexture, AlbumName, filename, (callback, path) =>
        {
            if (callback == false)
            {
                Debug.Log("Failed to save !");
            }
            else
            {
                Debug.Log("Photo is saved to Camera Roll on phone device.");

                Function_onSaved_Return.Invoke(); // Triggered [On Unity Event] in Visual Scripting
            }

        });

    }

    public void PickPhoto()
    {
    }


    public Texture2D GetTexture2DIOS(string path)
    {
        //Get Texture2D from path of an Image : all types JPG,JPEG,PNG,HEIC...
        Texture2D newText_ = NativeGallery.LoadImageAtPath(path, -1, true, true, false);
        return newText_;
    }


    public async void AskPermissionGal()
    {
        NativeGallery.Permission permissionResultGal = await NativeGallery.RequestPermissionAsync(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);
        permissionGal = permissionResultGal;

        if (permissionGal == NativeGallery.Permission.Granted)
        {
            PickPhotoGallery();
        }
    }
    

}
