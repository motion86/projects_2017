import React from 'react';
import ReactDOM from 'react-dom';
import App from './components/app';
import {Provider} from 'react-redux';
import store from './store';
import './app.css';

ReactDOM.render(
    //Provider is used to connect the store to the app
    <Provider store={store}>
        <App />
    </Provider> , 
    document.getElementById('root')
);