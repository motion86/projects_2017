import React, {Component} from 'react';
import './app.css';

class Profile extends Component {
    constructor(props){
        super(props);
        this.state = { }
    }
    
    render() {
        console.log('this.props', this.props);
        let artist = {name: '', followers: {total: ''}, images: [{url: ''}], genres:[]};
        artist = this.props.artist !== null ? this.props.artist : artist;

        return (
            <div className='profile'>
                <img src={artist.images[0].url} alt="Profile" className='profile-img' />
                <div className='profile-info'>
                    <div className='profile-name'>{artist.name}</div>
                    <div className='profile-followers'>{artist.followers.total} followers</div>
                    <div className='profile-genres'>
                        {
                            artist.genres.map((g, k) => {
                                g = g !== artist.genres[artist.genres.length -1] ? ` ${g},` : ` ${g}`;
                                return(
                                    <span key={k}>{g}</span>
                                );
                            })
                        }
                    </div>
                </div>
            </div>
        );
    }
}

export default Profile;