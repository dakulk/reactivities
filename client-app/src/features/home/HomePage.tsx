import React from "react"
import { Link } from "react-router-dom"
import { Container } from "semantic-ui-react"
export default function HomePage(){
    return (
        <Container stule={{marginTop:'7em'}}>
            <h1>Home</h1>
            <h2>GO To <Link to='/activities'>activities</Link></h2>
        </Container>
    )
}