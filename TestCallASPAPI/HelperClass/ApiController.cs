using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestCallASPAPI.HelperClass
{
    public class ApiController
    {
        private IResult<InstaUserShortList> ValueFollowers;

        private IResult<InstaUserShortList> ValueFollowing;

        public ObservableCollection<Model.Human> GetFollowers(string UserName)
        {
            ObservableCollection<Model.Human> FollowersCollection = 
                new ObservableCollection<Model.Human>();

            InstaUserShortList list = 
                this.ValueFollowers.Value;

            list.ForEach(x =>
            {
                FollowersCollection.Add(new Model.Human()
                {
                    Image = x.ProfilePicture,
                    UserName = x.UserName,
                    FullName = x.FullName,
                    Url = x.Pk
                });
            });
            return FollowersCollection;
        }
        public async Task<Tuple<int,int>> GetCountFollowersAndFollowing(string UserName)
        {
            this.ValueFollowers = await App.api.GetUserFollowersAsync(UserName, PaginationParameters.MaxPagesToLoad(1));
            this.ValueFollowing = await App.api.GetUserFollowingAsync(UserName, PaginationParameters.MaxPagesToLoad(1));
            return Tuple.Create(this.ValueFollowers.Value.Count, this.ValueFollowing.Value.Count);
        }
        public async Task<IResult<InstaCommentList>> GetComments(string MediaID)
        {
            return await App.api.GetMediaCommentsAsync(MediaID, PaginationParameters.MaxPagesToLoad(1));
        }
        public async Task<IResult<InstaLikersList>> GetLikes(string MediaId)
        {
            return await App.api.GetMediaLikersAsync(MediaId);
        }
        public async Task PostLike(string MediaId)
        {
            await App.api.LikeMediaAsync(MediaId);
        }
        public async Task UnPostLike(string MediaId)
        {
            await App.api.UnLikeMediaAsync(MediaId);
        }
    }
}
