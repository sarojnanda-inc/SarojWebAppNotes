import React, { Component } from 'react';
import axios from 'axios';
import {
    Container, Col, Form,
    FormGroup, Label, Input,
    Button
} from 'reactstrap';
export class Home extends Component {
    static displayName = Home.name;
    constructor(props) {
        super(props);
        this.state = { loading: true, userName:"", password:"" };
    }
    setUserName = (value) => {
        this.setState({ userName:value });
    }
    setPassword = (value) => {
        this.setState({ password:value });
    }
    handleSubmit = (event) => {
        event.preventDefault();

        try {
            const options = {
                headers: { 'Authorization': btoa(this.state.userName + ':' + this.state.password) }
            };
            axios.post('/home/Authenticate',  { Username: this.state.userName, Password: this.state.password } ,options)
                .then(response => {
                    sessionStorage.setItem("user", JSON.stringify(response.data));
                    this.props.history.push("/notes");
                }
                )
                .catch(error => {
                    this.setState({ errorMessage: error.message });
                    console.error('There was an error!', error);
                });
        } catch (e) {
            alert(e.message);
        }
    }

    render() {
        return (
            <Container className="App">
                <h2>Sign In</h2>
                <Form className="form" onSubmit={this.handleSubmit}>
                    <Col>
                        <FormGroup>
                            <Label>Email</Label>
                            <Input
                                type="email"
                                name="email"
                                id="exampleEmail"
                                placeholder="myemail@email.com"
                                onChange={e => this.setUserName(e.target.value)}
                            />
                        </FormGroup>
                    </Col>
                    <Col>
                        <FormGroup>
                            <Label for="examplePassword">Password</Label>
                            <Input
                                type="password"
                                name="password"
                                id="examplePassword"
                                placeholder="********"
                                onChange={e => this.setPassword(e.target.value)}
                            />
                        </FormGroup>
                    </Col>
                    <Button >Submit</Button>
                </Form>
            </Container>
        );
    }
}
