using System.Linq.Expressions;
using Camera.BLL.Interfaces;
using Camera.DAL.Interfaces.Repositories;
using Joint.Data.Models;

namespace Camera.BLL.Services;

public class CameraService : ICameraService
{
    private readonly ICameraRepository _repository;
    private readonly ICameraNotifiesRepository _notifiesRepository;

    public CameraService(ICameraRepository repository,
        ICameraNotifiesRepository cameraNotifiesRepository)
    {
        this._repository = repository;
        _notifiesRepository = cameraNotifiesRepository;
    }
    public void Save(Joint.Data.Models.Camera item)
    {
        this.Save(new List<Joint.Data.Models.Camera>() { item });
    }

    public void Save(IList<Joint.Data.Models.Camera> items)
    {
        foreach (var item in items)
        {
            var old = _repository.Read(i => i.Id == item.Id);

            if (old.Any())
            {
                this._repository.Update(item);
            }
            else
            {
                this._repository.Create(item);
            }

        }
        this._repository.Save();
    }

    public IList<Joint.Data.Models.Camera> Read()
    {
        return this._repository.Read(i => true).ToList();
    }

    public IList<Joint.Data.Models.Camera> Read(Expression<Func<Joint.Data.Models.Camera, bool>> where)
    {
        return this._repository.Read(where).ToList();
    }

    public Joint.Data.Models.Camera Read(object id)
    {
        return this._repository.Read(i => i.Id == (int)id).FirstOrDefault();
    }

    public void Delete(Joint.Data.Models.Camera item)
    {
        this.Delete(new List<Joint.Data.Models.Camera>() { item });
    }

    public void Delete(IList<Joint.Data.Models.Camera> items)
    {
        foreach (var item in items)
        {
            var old = _repository.Read(i => i.Id == item.Id);

            if (old.Any())
            {
                var notifies = _notifiesRepository.Read(i => i.CameraId == item.Id);
                if (notifies.Any())
                {
                    _notifiesRepository.Delete(notifies.First());
                }
                this._repository.Delete(old.FirstOrDefault());

            }
        }
        this._repository.Save();
    }
    public void Dispose()
    {
        this._repository.Dispose();
    }

}