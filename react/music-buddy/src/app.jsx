import React, { Component } from 'react';
import { FormGroup, FormControl, InputGroup, Glyphicon } from 'react-bootstrap';
import Profile from './profile';
import Gallery from './gallery';
import './app.css';


class App extends Component {
    constructor(props){
        super(props);
        this.state ={
            query: '',
            artist: null,
            tracks: []
        }
    }

    search() {
        console.log('this.state', this.state);        
        const BASE_URL = 'https://api.spotify.com/v1/search?';
        let FETCH_URL = `${BASE_URL}q=${this.state.query}&type=artist&limit=1`;
        const ALBUM_URL = 'https://api.spotify.com/v1/artists/'
        var accessToken = 'BQC4MvHIuFVIDnUB-gkcHjJ_7_2mHpODoubgmEW0QrZM-W1oUHGNJwBBg2mNVRQgPWc4EnKYOIlVkCkdxGAC3fvCrgsdy0FLSzbxg3NXzm44Ebl0EFrj0prGWYt6yrvfgfHDv9BOtsIz08DESVruUkFkPxOy6S9gTtnT'
        var myHeaders = new Headers();
    
        var myOptions = {
          method: 'GET',
          headers:  {
            'Authorization': 'Bearer ' + accessToken
         },
          mode: 'cors',
          cache: 'default'
        };
    
        fetch(FETCH_URL, myOptions )
          .then(response => response.json())
          .then(json =>{
            const artist = json.artists.items[0];
            console.log('artist',artist);
            this.setState({artist})

            FETCH_URL = `${ALBUM_URL}${artist.id}/top-tracks?country=US&`
            fetch(FETCH_URL, myOptions)
            .then(response => response.json())
            .then(json => {
                console.log('artist\'s top tracks:', json);
                const { tracks } = json;
                this.setState({tracks});
            });
          });
    }

    render() {
        return (
            <div className='app'>
                <div>
                    <div className='app-title'>Music Buddy</div>
                </div>
                <FormGroup>
                    <InputGroup>
                        <FormControl
                            type='text'
                            placeholder='Search for an artist..'
                            value={this.state.query}
                            onChange={e => this.setState({query: e.target.value})}
                            onKeyPress={e => e.key === 'Enter' ? this.search() : false}
                        />
                        <InputGroup.Addon onClick={() => this.search()}>
                            <Glyphicon glyph='search' />
                        </InputGroup.Addon>
                    </InputGroup>
                </FormGroup>
                {
                    this.state.artist !== null
                    ?   <div>
                            <Profile artist={this.state.artist}/>
                            <Gallery tracks={this.state.tracks} />
                        </div>
                    :   <div></div>
                }
            </div>
        );
    }
}

export default App;