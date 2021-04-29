import  React, {useState, useEffect } from "react";
import axios from 'axios';
import CRUDTable, {
    Fields,
    Field,
    CreateForm,
    UpdateForm,
    DeleteForm
} from "react-crud-table";



const DescriptionRenderer = ({ field }) => <textarea {...field} />;

let notes = [
    {
        id: 1,
        title: "Create an example",
        description: "Create an example of how to use the component"
    },
    {
        id: 2,
        title: "Improve",
        description: "Improve the component!"
    }
];

const SORTERS = {
    NUMBER_ASCENDING: mapper => (a, b) => mapper(a) - mapper(b),
    NUMBER_DESCENDING: mapper => (a, b) => mapper(b) - mapper(a),
    STRING_ASCENDING: mapper => (a, b) => mapper(a).localeCompare(mapper(b)),
    STRING_DESCENDING: mapper => (a, b) => mapper(b).localeCompare(mapper(a))
};

const getSorter = data => {
    const mapper = x => x[data.field];
    let sorter = SORTERS.STRING_ASCENDING(mapper);

    if (data.field === "id") {
        sorter =
            data.direction === "ascending"
                ? SORTERS.NUMBER_ASCENDING(mapper)
                : SORTERS.NUMBER_DESCENDING(mapper);
    } else {
        sorter =
            data.direction === "ascending"
                ? SORTERS.STRING_ASCENDING(mapper)
                : SORTERS.STRING_DESCENDING(mapper);
    }

    return sorter;
};

let count = notes.length;
const service = {
    fetchItems: payload => {
        let result = Array.from(notes);
        result = result.sort(getSorter(payload.sort));
        return Promise.resolve(result);
    },
    create: note => {
        count += 1;
        notes.push({
            ...note,
            id: count
        });
        return Promise.resolve(note);
    },
    update: data => {
        const note = notes.find(t => t.id === data.id);
        note.title = data.title;
        note.description = data.description;
        return Promise.resolve(note);
    },
    delete: data => {
        const note = notes.find(t => t.id === data.id);
        notes = notes.filter(t => t.id !== note.id);
        return Promise.resolve(note);
    }
};

const styles = {
    container: { margin: "auto", width: "fit-content" }
};

export const Notes = (props) => {
    const [notes, setNotes] = useState([]);
    useEffect(() => {
        retrieveNotes();
    }, []);
    const retrieveNotes = () => {
        let user = JSON.parse(sessionStorage.getItem("user"));
        const options = {
            headers: { 'Authorization': btoa(user.username + ':' + user.password) }
        };
        axios.get(`/home/GetAllNotes/${user.id}`, options).then(response => {
            setNotes(response.data)
        }
        ).catch(error => {
            
            console.error('There was an error!', error);
        });
    };
        return (
            <div style={styles.container}>
                <CRUDTable
                    caption="notes"
                    fetchItems={payload => service.fetchItems(payload)}
                >
                    <Fields>
                        <Field name="id" label="Id" hideInCreateForm />
                        <Field name="title" label="Title" placeholder="Title" />
                        <Field
                            name="description"
                            label="Description"
                            render={DescriptionRenderer}
                        />
                    </Fields>
                    <CreateForm
                        title="note Creation"
                        message="Create a new note!"
                        trigger="Create note"
                        onSubmit={note => service.create(note)}
                        submitText="Create"
                        validate={values => {
                            const errors = {};
                            if (!values.title) {
                                errors.title = "Please, provide note's title";
                            }

                            if (!values.description) {
                                errors.description = "Please, provide note's description";
                            }

                            return errors;
                        }}
                    />

                    <UpdateForm
                        title="note Update Process"
                        message="Update note"
                        trigger="Update"
                        onSubmit={note => service.update(note)}
                        submitText="Update"
                        validate={values => {
                            const errors = {};

                            if (!values.id) {
                                errors.id = "Please, provide id";
                            }

                            if (!values.title) {
                                errors.title = "Please, provide note's title";
                            }

                            if (!values.description) {
                                errors.description = "Please, provide note's description";
                            }

                            return errors;
                        }}
                    />

                    <DeleteForm
                        title="Note Delete Process"
                        message="Are you sure you want to delete the note?"
                        trigger="Delete"
                        onSubmit={note => service.delete(note)}
                        submitText="Delete"
                        validate={values => {
                            const errors = {};
                            if (!values.id) {
                                errors.id = "Please, provide id";
                            }
                            return errors;
                        }}
                    />
                </CRUDTable>
            </div>
        )
    };
