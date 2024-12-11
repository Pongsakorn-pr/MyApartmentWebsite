import React, { useState, useEffect } from "react";
import { Button, Table } from 'react-bootstrap';
import { PencilSquare, Trash, Printer } from 'react-bootstrap-icons';
import { useNavigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

const DataPage = () => {
    const [data, setData] = useState([]);
    const [error, setError] = useState(null);
    const navigate = useNavigate();
    const deleteData = async (item) => {
        try {
            // Use the actual ID of the item
            var id = item.bill_id + 1;
            await axios.delete(`https://localhost:7054/api/Apartment/${id}`);
            console.log("Data Deleted:", item);
            // Update UI/state in the parent component (e.g., remove the deleted item from the list)
            // You can achieve this by passing a function to the deleteData function 
            // to update the state in the parent component.
        } catch (error) {
            console.error("Error deleting data:", error);
            // Display an error message to the user 
        }
    };
    const handleEdit = (item) => {
        console.log(item);
        navigate(`/Edit/${item.bill_id}`, { state: { data: item } });
        console.log(item.bill_id);
    };
    useEffect(() => {
        // Fetch data from the backend API
        fetch("https://localhost:7054/api/Apartment") // Adjust API URL accordingly
            .then((response) => {
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                return response.json();
            })
            .then((data) => {
                setData(data); // Set the data in the state
            })
            .catch((error) => {
                setError(error.message); // Handle errors
            });
    }, []);

    return (
        <div className="container mt-4">
            <h1 className="mb-4">ข้อมูลหอพักทั้งหมด</h1>
            {error && <div className="alert alert-danger">{error}</div>} {/* Display error message */}

            <Table striped bordered hover responsive >
                <thead className="table-dark" >
                    <tr>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>เลขที่บิล</th>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>เลขที่ห้อง</th>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>ประจำเดือน/ปี</th>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>ค่าห้อง</th>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>ค่ามิเตอร์เดือนก่อน</th>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>ค่ามิเตอร์เดือนปัจจุบัน</th>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>ส่วนต่างมิเตอร์</th>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>ค่าน้ำต่อหน่วย</th>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>ค่าขยะ</th>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>ค่าอื่นๆ</th>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>รวม</th>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>Print</th>
                        <th style={{ whiteSpace: 'nowrap', textAlign: 'center' }}>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {data.map((item, rowIndex) => (
                        <tr key={rowIndex}>
                            <td>{item.bill_id}</td>
                            <td>{item.room_number}</td>
                            <td>{item.month}/{item.year}</td>
                            <td>{item.room_rent}</td>
                            <td>{item.previous_meter_month}</td>
                            <td>{item.water_reading_meter}</td>
                            <td>{item.water_diff}</td>
                            <td>{item.water_unit_fees}</td>
                            <td>{item.garbage_fees}</td>
                            <td>{item.other_fees}</td>
                            <td>{item.total_amount}</td>
                            <td><Button variant="primary" size ="lg" ><Printer /></Button></td>
                            <td>
                                <div style={{ display: 'flex', justifyContent: 'center' }}> {/* Center buttons horizontally */}
                                    <Button
                                        variant="warning"
                                        className="me-2"
                                        size="lg"
                                        style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}
                                        onClick={() => handleEdit(item)} 
                                    >
                                        <PencilSquare />
                                    </Button>
                                    <Button
                                        variant="danger"
                                        className="me-2"
                                        size="lg"
                                        style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}
                                        onClick={() => deleteData(item)}
                                    >
                                        <Trash />
                                    </Button>
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};
const style = {
    // Table custom styles can go here if needed
    tableBill: {
        width: '100%',
        marginTop: '20px',
    },
};
export default DataPage;
